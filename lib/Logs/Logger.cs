using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.CompilerServices;
using Lib;
using Lib.IO;

namespace Lib.Logs
{
    public abstract class Logger
    {
        [ChainCaseName] public Level Level { get; init; }
        [ChainCaseName] public string FilePath { get; init; }
        [ChainCaseName] public string Format { get; init; }
        [ChainCaseName] public FileSize MaxSize { get; init; }
        [ChainCaseName] public int RotationCount { get; init; }
        [ChainCaseName] public string RotateSuffix { get; init; }
        /*
        readonly Settings _setting;
        string _format;
        LogWriter _writer;
        public virtual Settings Settings
        {
            get => _setting;
            init
            {
                _setting = value == null || value.IsEmpty ? DefaultSettings : value;
                try
                {
                    Initialize();
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("failed to create logger.");
                    System.Diagnostics.Trace.WriteLine(ex);
                }
            }
        }
        public abstract Settings DefaultSettings { get; }
        public virtual string[] FormatParameters { get; } = Array.Empty<string>();
        ~Logger() => _writer?.Dispose();
        protected virtual void Initialize()
        {
            _format = GenerateFormat();
            _writer = CreateWriter();
        }
        protected virtual string GenerateFormat() => ReplaceKeywords(Settings.Format, FormatParameters);
        protected abstract LogWriter CreateWriter();
        protected abstract object[] GetLogParameters(Level level, string callsource, string message);
        public void Out(Level level, [CallerMemberName] string callsource = "", string format, params object[] args)
        {
            var message = string.Format(format, args);
            try
            {
                if (level < Settings.Level) return;
                if (Settings.MaxSize.Value <= _writer.Length) Lotate();
                _writer.WriteLine(_format, GetLogParameters(level, message));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("failed to output log.");
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }
        public void Out(Level level, object obj) => Out(level, "{0}", obj);
        public virtual void Lotate()
        {
            var file = new FileInfo(Settings.FilePath);
            var pt = file.Directory.FullName;
            var ex = file.Extension;
            var fn = file.Name.TrimEnd(ex);
            var rotate = pt + "\\" + fn + ReplaceKeywords(Settings.RotateSuffix, new[] { "#", "date-time" }) + ex;
            foreach(var f in file.Directory.GetFiles(fn + "*" + ex))
            {

            }
        }
        public static string ReplaceKeywords(string input, string[] keys)
        {
            for (var i = 0; i < keys.Length; i++) input = Regex.Replace(input, "{" + keys[i] + "([^}]*)}", "{" + i + "$1}");
            return input;
        }
        public static string ReplaceKeywords(string input, string[] keys, params object[] parameters) =>
            string.Format(ReplaceKeywords(input, keys), parameters);
    }
    public class DefaultLogger : Logger
    {
        const string ConsoleMode = "console";
        const string DebugMode = "debug";
        public override Settings DefaultSettings => new()
        {
            Level = Level.Info,
            FilePath = ConsoleMode,
            Format = @"{now:yyyy/MM/dd HH:mm:ss.fff},[{level,-5}],{thread},{category},{message}",
            MaxSize = "100M",
            RotationCount = 0,
            RotateSuffix = "_{#}",
        };
        public override string[] FormatParameters => new[] { "now", "level", "thread", "category", "message", };
        public bool IsConsoleMode => string.IsNullOrEmpty(Settings.FilePath) || Settings.FilePath == ConsoleMode;
        public bool IsDebugMode => string.IsNullOrEmpty(Settings.FilePath) || Settings.FilePath == DebugMode;
        protected override LogWriter CreateWriter() => 
            Try.Of(() =>
                IsConsoleMode ? LogWriter.OnConsole :
                IsDebugMode ? LogWriter.OnDebug :
                LogWriter.ToFile(Settings.FilePath))
            .Catch(ex => LogWriter.Empty)
            .Invoke();
        protected override object[] GetLogParameters(Level level, string message) => new object[]
        {
            DateTime.Now,
            level,
            Thread.CurrentThread.ManagedThreadId,

            message,
        };
        */
    }
}
