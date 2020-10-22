using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class FileArguments : Arguments
    {
        const int ENCODE_SHIFTJIS = 932;
        const int ENCODE_UTF8 = 65001;

        [Command]
        [Command("cc")]
        [CommandValue]
        [Detail("read by char code.")]
        public int CharCode { get; set; } = ENCODE_UTF8;
        [Command("932")]
        [Command("shift-jis")]
        [Detail("equal /charcode 932")]
        public bool ShiftJis 
        {
            get => CharCode == ENCODE_SHIFTJIS;
            set => CharCode = value ? ENCODE_SHIFTJIS : ENCODE_UTF8; 
        }
        [Command("65001")]
        [Command("utf-8")]
        [Detail("equal /charcode 65001")]
        public bool Utf8
        {
            get => CharCode == 65001;
            set => CharCode = value ? ENCODE_UTF8 : ENCODE_SHIFTJIS;
        }
        [Command("s")]
        [Detail("find in sub directories.")]
        public bool Subs { get; set; }

        public virtual FileMode Mode { get; set; } = FileMode.Open;
        public virtual FileAccess Access { get; set; } = FileAccess.Read;
        public virtual FileShare Share { get; set; } = FileShare.ReadWrite;
        public virtual bool DefaultAllFiles => false;
        public override List<string> Values => base.Values.Count > 0 ? base.Values : DefaultAllFiles ? new List<string> { "*" } : base.Values;
        internal FileArguments(IEnumerable<string> args, string option) : base(args, option) { }
        public virtual IEnumerable<FileSystemInfo> GetFileSystems()
        {
            foreach (var v in this.Values)
            {
                var s = v.Split('\\');
                var d = string.Join("\\", s.Take(s.Length - 1));
                if (string.IsNullOrEmpty(d)) d = Directory.GetCurrentDirectory();
                var p = s.Last();
                var di = new DirectoryInfo(d);
                foreach (var f in GetFileSystems(di, p)) yield return f;
            }
        }
        protected virtual IEnumerable<FileSystemInfo> GetFileSystems(DirectoryInfo di, string pattern)
        {
            if (!di.Exists) yield break;
            var fs = di.GetFiles(pattern);
            if (fs.Length > 0)
            {
                yield return di;
                foreach (var f in fs) yield return f;
            }
            if (Subs) foreach (var d in di.GetDirectories()) foreach (var f in GetFileSystems(d, pattern)) yield return f;
        }
        public virtual IEnumerable<FileInfo> GetFiles() => GetFileSystems().Where(_ => (_.Attributes & FileAttributes.Directory) == 0).Select(_ => (FileInfo)_);
        public virtual IEnumerable<Stream> GetStreams(int buffersize)
        {
            foreach (var f in GetFiles().Select(_ => new FileStream(_.FullName, Mode, Access, Share, buffersize))) using (var f_ = f) yield return f_;
        }
        public virtual IEnumerable<Stream> GetStreams() => GetStreams(10 * 1024 * 1024);
        public virtual IEnumerable<TextReader> GetReaders()
        {
            if (Console.IsInputRedirected)
            {
                yield return Console.In;
                yield break;
            }
            foreach (var r in GetStreams().Select(_ => new StreamReader(_, Encoding.GetEncoding(CharCode)))) yield return r;
        }
        public virtual IEnumerable<string> GetLines()
        {
            string line;
            foreach (var r in GetReaders()) while ((line = r.ReadLine()) != null) yield return line;
        }

        public new static FileArguments Load() => Load(DEFAULT_OPTION_CHAR);
        public new static FileArguments Load(string option) => Load(Environment.GetCommandLineArgs().Skip(1), option);
        public new static FileArguments Load(IEnumerable<string> args) => Load(args, DEFAULT_OPTION_CHAR);
        public new static FileArguments Load(IEnumerable<string> args, string option) => new FileArguments(args, option);
        public new static FileArguments<T> Load<T>() => Load<T>(DEFAULT_OPTION_CHAR);
        public new static FileArguments<T> Load<T>(string option) => Load<T>(Environment.GetCommandLineArgs().Skip(1), option);
        public new static FileArguments<T> Load<T>(IEnumerable<string> args) => Load<T>(args, DEFAULT_OPTION_CHAR);
        public new static FileArguments<T> Load<T>(IEnumerable<string> args, string option) => new FileArguments<T>(args, option);
    }
    public class FileArguments<T> : FileArguments
    {
        T _options;
        public T Options => _options;
        internal FileArguments(IEnumerable<string> args, string option) : base(args, option) { }
        protected override void Initialize()
        {
            base.Initialize();
            _options = (T)typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { });
            _map.Add(_options);
        }
    }
}
