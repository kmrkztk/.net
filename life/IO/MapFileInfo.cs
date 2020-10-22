using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;
using Lib;
using Lib.Reflection;

namespace life.IO
{
    public class MapFileInfo
    {
        static readonly Dictionary<string, IMapFileExtension> _extensions
             = ReflectiveEnumerator.GetEnumerableInstanceOfType<IMapFileExtension>().ToDictionary(_ => _.Extension);
        public static IMapFileExtension Extension(string ext) => _extensions[ext];
        public static MapFileInfo Save(string filename, Map map, params string[] comments) => Save(filename, map, null, comments);
        public static MapFileInfo Save(string filename, Map map, string name, params string[] comments) => Save(filename, map, name, null, comments);
        public static MapFileInfo Save(string filename, Map map, string name, string author, params string[] comments) => Save(new FileInfo(filename), map, name, author, comments);
        public static MapFileInfo Save(string filename, IMapFileExtension ext, Map map, params string[] comments) => Save(filename, ext, map, null, comments);
        public static MapFileInfo Save(string filename, IMapFileExtension ext, Map map, string name, params string[] comments) => Save(filename, ext, map, name, null, comments);
        public static MapFileInfo Save(string filename, IMapFileExtension ext, Map map, string name, string author, params string[] comments) => Save(new FileInfo(filename), ext, map, name, author, comments);
        public static MapFileInfo Save(FileInfo fi, Map map, params string[] comments) => Save(fi, map, null, comments);
        public static MapFileInfo Save(FileInfo fi, Map map, string name, params string[] comments) => Save(fi, map, name, null, comments);
        public static MapFileInfo Save(FileInfo fi, Map map, string name, string author, params string[] comments) => Save(fi, null, map, name, author, comments);
        public static MapFileInfo Save(FileInfo fi, IMapFileExtension ext, Map map, params string[] comments) => Save(fi, ext, map, null, comments);
        public static MapFileInfo Save(FileInfo fi, IMapFileExtension ext, Map map, string name, params string[] comments) => Save(fi, ext, map, name, null, comments);
        public static MapFileInfo Save(FileInfo fi, IMapFileExtension ext, Map map, string name, string author, params string[] comments)
        {
            var mi = new MapFileInfo(fi, ext)
            {
                Name = name,
                Author = author,
                Comments = comments?.ToList() ?? new List<string>(),
                _size = map.Size,
            };
            mi.Save(map);
            return mi;
        }
        public static Map Load(string filename) => Load(new FileInfo(filename));
        public static Map Load(FileInfo fi) => new MapFileInfo(fi).Load();

        readonly Assembly _assembly;
        readonly IMapFileExtension _ext;
        public string Name { get; set; }
        public string Author { get; set; }
        public List<string> Comments { get; private set; } = new List<string>();
        Size _size;
        public int Width => _size.Width;
        public int Height => _size.Height;
        public Size Size => _size;
        public string ResouceName { get; private set; }
        public string FullPath { get; private set; }
        public bool Savable => FullPath != null;
        public MapFileInfo(string filename) : this(new FileInfo(filename)) { }
        public MapFileInfo(string filename, IMapFileExtension ext) : this(new FileInfo(filename), ext) { }
        public MapFileInfo(FileInfo fi) : this(fi, null) { }
        public MapFileInfo(FileInfo fi, IMapFileExtension ext)
        {
            _ext = ext ?? _extensions[fi.Extension];
            FullPath = fi.FullName;
            Refresh();
        }
        public MapFileInfo(Assembly assembly, string resource, string ext) : this(assembly, resource, _extensions[ext]) { }
        public MapFileInfo(Assembly assembly, string resource, IMapFileExtension ext)
        {
            _assembly = assembly;
            _ext = ext ?? throw new ArgumentNullException();
            ResouceName = resource;
            Refresh();
        }
        public void Clear()
        {
            Name = null;
            Author = null;
            Comments.Clear();
        }
        public void Save(Map map)
        {
            if (!Savable) return;
            var sb = new StringBuilder();
            sb.Append(_ext.Format(this));
            sb.Append(_ext.Format(map));
            var buf = Encoding.UTF8.GetBytes(sb.ToString());
            using (var s = CreateStream()) s.Write(buf, 0, buf.Length);
        }
        public Map Load()
        {
            using (var s = OpenStream()) using (var sr = new StreamReader(s)) return _ext.ReadMap(sr);
        }
        public void Refresh()
        {
            Clear();
            using (var s = OpenStream()) Refresh(s);
        }
        public void Refresh(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            {
                Name = _ext.ReadName(sr);
                Author = _ext.ReadAuthor(sr);
                Comments.AddRange(_ext.ReadComments(sr));
                sr.DiscardBufferedData();
                stream.Seek(0, SeekOrigin.Begin);
                _size = _ext.ReadSize(sr);
            }
        }
        Stream OpenStream() => Savable ? new FileStream(FullPath, FileMode.Open) : _assembly.GetManifestResourceStream(ResouceName);
        Stream CreateStream() => Savable ? new FileStream(FullPath, FileMode.Create) : throw new InvalidOperationException();
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(Name ?? "no name");
            sb.Append(" ");
            if (Author != null) sb.AppendFormat("[{0}] ", Author);
            sb.AppendFormat("({0}x{1})", Width, Height);
            return sb.ToString();
        }
    }
}
