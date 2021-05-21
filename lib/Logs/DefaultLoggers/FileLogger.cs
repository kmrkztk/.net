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
        static readonly string _assembly = System.Reflection.Assembly.GetEntryAssembly().GetName().Name;
        static readonly string[] _keywords = new[]
        {
            "assembly-name",
            "level",
            "datetime",
            "age",
        };
        [LowerName] public string FilePath { get; init; } = @".\{assembly-name}.log";
        [LowerName] public FileRotation Rotation { get; init; } = new FileRotation();
        public class FileRotation
        {
            [LowerName] public FileSize Size { get; init; } = "1024";
            [LowerName] public int Count { get; init; } = 0;
            public string Suffix => ".{age:" + new string('0', Count.ToString().Length) + "}";
        }
        DateTime _generating;
        string _current;
        string _filename;
        string _rotate;
        string _pattern;
        bool _daily;

        StreamWriter _writer;
        ~FileLogger() => _writer.Dispose();
        public override void Initialize()
        {
            base.Initialize();

            _filename = FilePath
                .Replace("{datetime}", "{datetime:yyyyMMdd}")
                .ReplaceKeywords(
                    _keywords[0..2],
                    new[] { _assembly, Level.ToString().ToLower(), });
            _current = GenerateFile();
            _writer = OpenStream();

            var current = new FileInfo(_current);
            var ex = current.Extension;
            var suffix = Rotation.Suffix;
            _rotate = current.FullName.TrimEnd(ex) + suffix + ex;

            _pattern = new FileInfo((_filename.TrimEnd(ex) + suffix + ex).ReplaceKeywords(_keywords[2..4], new[] { "*", "*", })).Name;
            _daily = Regex.IsMatch(FilePath, "{datetime[^}]*}");
        }
        string GenerateFile() => _filename.ReplaceKeywords(_keywords[2]).Format(_generating = Now);
        StreamWriter OpenStream() => new(_current, true);
        public void Rotate()
        {
            void rotate(string filename, int index)
            {
                var dst = _rotate.ReplaceKeywords(_keywords[2..4]).Format(_generating, index);
                if (File.Exists(dst)) rotate(dst, index + 1);
                File.Move(filename, dst);
            }
            _writer?.Dispose();
            _current = GenerateFile();
            if (File.Exists(_current)) rotate(_current, 0);
            var current = new FileInfo(_current);
            var files =
                current.Directory
                .GetFiles(_pattern)
                .OrderBy(_ => _.LastWriteTime)
                .ToList();
            files.Take(files.Count - Rotation.Count).Foreach(_ => _.Delete());
            _writer = OpenStream();
        }
        protected override void Out(string format, params object[] args)
        {
            if (_writer.BaseStream.Length >= Rotation.Size) Rotate();
            if (_daily && _generating.Date != Now.Date) Rotate();
            _writer.WriteLine(format, args);
            _writer.Flush();
        }
    }
}
