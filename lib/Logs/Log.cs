using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Lib.Jsons;
using Lib.Reflection;
using Lib.Logs.DefaultLoggers;

namespace Lib.Logs
{
    public class Log
    {
        class Option
        {
            [Command("log.options.filename")] [CommandValue] public string FileName { get; set; } = @".\log.settings.json";
            public static Option Load() => Arguments<Option>.Load().Options;
        }
        static Log()
        {
            var loggers = typeof(ILogger)
                .GetEnumerableOfType()
                .Where(_ => !_.IsAbstract)
                .Where(_ => !_.IsInterface)
                .SelectMany(type => NameAttribute.GetTypeNames(type)
                .Concat(new[] { type.Name, })
                .Select(n => n.ToLower())
                .Distinct()
                .Select(name => (name, type)))
                .ToDictionary(_ => _.name, _ => _.type);
            var option = Option.Load();
            try
            {
                var settings = Json.Load(new FileStream(option.FileName, FileMode.Open));
                Listener = settings
                    .AsArray()
                    .Select(_ => (settings:_, key:_["type"]?.Value?.ToLower() ?? "default"))
                    .Where(_ => loggers.ContainsKey(_.key))
                    .Select(_ => _.settings.Cast(loggers[_.key]))
                    .Cast<ILogger>()
                    .Where(_ =>
                    {
                        try
                        {
                            _.Initialize();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Trace.WriteLine("failed to create logger.");
                            System.Diagnostics.Trace.WriteLine(ex);
                            return false;
                        }
                    })
                    .ToList();
            }
            catch(Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("failed to create logger. settings:" + option.FileName);
                System.Diagnostics.Trace.WriteLine(ex);
                Listener = new() { new ConsoleLogger(), new DebugLogger(), };
                Listener.Foreach(_ => _.Initialize());
            }
        }
        public static List<ILogger> Listener { get; }
        public static Log Of(Level level,
            [CallerMemberName] string name = "",
            [CallerFilePath] string filepath = "",
            [CallerLineNumber] int linenumber = 0) => new()
            {
                Level = level,
                Caller = new()
                {
                    MemberName = name,
                    FilePath = filepath,
                    LineNumber = linenumber,
                }
            };
        public static Log Trace([CallerMemberName] string name = "", [CallerFilePath] string filepath = "", [CallerLineNumber] int linenumber = 0) => Of(Level.Trace, name, filepath, linenumber);
        public static Log Debug([CallerMemberName] string name = "", [CallerFilePath] string filepath = "", [CallerLineNumber] int linenumber = 0) => Of(Level.Debug, name, filepath, linenumber);
        public static Log Info ([CallerMemberName] string name = "", [CallerFilePath] string filepath = "", [CallerLineNumber] int linenumber = 0) => Of(Level.Info , name, filepath, linenumber);
        public static Log Warn ([CallerMemberName] string name = "", [CallerFilePath] string filepath = "", [CallerLineNumber] int linenumber = 0) => Of(Level.Warn , name, filepath, linenumber);
        public static Log Error([CallerMemberName] string name = "", [CallerFilePath] string filepath = "", [CallerLineNumber] int linenumber = 0) => Of(Level.Error, name, filepath, linenumber);
        public static Log Fatal([CallerMemberName] string name = "", [CallerFilePath] string filepath = "", [CallerLineNumber] int linenumber = 0) => Of(Level.Fatal, name, filepath, linenumber);



        public Level Level { get; init; }
        public LogCaller Caller { get; init; }
        public void Out(string format, params object[] args) { foreach (var _ in Listener) _.Out(Level, Caller, string.Format(format, args)); }
        public void Out(object obj) => Out("{0}", obj);
    }
}
