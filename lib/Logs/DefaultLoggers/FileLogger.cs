using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lib;
using Lib.IO;

namespace Lib.Logs.DefaultLoggers
{
    [Name("default")]
    public class FileLogger : Logger
    {
        const string KeywordAssembly = "assembly-name";
        const string KeywordLevel = "level";
        const string KeywordDateTime = "datetime";
        const string KeywordAge = "age";
        static string GetKeyword(string keyword, string format = "") => "{" + keyword + format + "}";
        static readonly string _assembly = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
        [LowerName] public string FilePath { get; init; } = @".\" + GetKeyword(KeywordAssembly) + ".log";
        [LowerName] public FileRotation Rotation { get; init; } = new FileRotation();
        public class FileRotation
        {
            [LowerName] public FileSize Size { get; init; } = "10M";
            [LowerName] public int Count { get; init; } = 0;
            [LowerName] public bool Daily { get; init; } = false;
            [LowerName] public string DateTimeFormat { get; init; } = "." + GetKeyword(KeywordDateTime, ":yyyyMMdd");
            public string Suffix => "." + GetKeyword(KeywordAge, ":" + new string('0', Count.ToString().Length));
        }
        DateTime _generating;
        string _filename;
        string _pattern;
        string _datetime;
        string _age;
        string _ex;

        StreamWriter _writer;
        ~FileLogger() => _writer?.Dispose();
        public override void Initialize()
        {
            base.Initialize();

            _filename = FilePath.ReplaceKeywords(new[] { KeywordAssembly, KeywordLevel, }).Format(_assembly, Level.ToString().ToLower());
            _ex = Path.GetExtension(_filename);
            _datetime = Rotation.Daily ? Rotation.DateTimeFormat : "";
            _age = Rotation.Suffix;
            _pattern = 
                Path.GetFileNameWithoutExtension(_filename) + 
                _datetime.ReplaceKeywords(KeywordDateTime, "*") + 
                _age.ReplaceKeywords(KeywordAge, "*") +
                _ex;
            _datetime = _datetime.ReplaceKeywords(KeywordDateTime);
            _age = _age.ReplaceKeywords(KeywordAge);
            Refresh(false);
        }
        public void Refresh(bool rotation = true)
        {
            _writer?.Dispose();
            _generating = DateTime.Now;
            var fn = Rotation.Daily ? _filename.TrimEnd(_ex) + _datetime.Format(_generating) + _ex : _filename;
            if (rotation)
            {
                var rn = fn.TrimEnd(_ex) + _age + _ex;
                void rotate(string filename, int index)
                {
                    var dst = rn.Format(index);
                    if (File.Exists(dst)) rotate(dst, index + 1);
                    File.Move(filename, dst);
                }
                if (File.Exists(fn)) rotate(fn, 0);
            }
            var fi = new FileInfo(fn);
            var di = fi.Directory;
            if (!di.Exists) di.Create();
            var files = di
                .GetFiles(_pattern)
                .OrderBy(_ => _.LastWriteTime)
                .ToList();
            files.Take(files.Count - Rotation.Count).Foreach(_ => _.Delete());
            _writer = new(fn, true);
        }
        protected override void Out(string message)
        {
            if (_writer.BaseStream.Length >= Rotation.Size) Refresh();
            if (Rotation.Daily && _generating.Date != DateTime.Now.Date) Refresh();
            _writer.WriteLine(message);
            _writer.Flush();
        }
    }
}
