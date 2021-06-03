using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Runtime.CompilerServices;
using Lib.Configuration;
using Lib.Jsons;
using Lib.Reflection;
using Lib.Logs.DefaultLoggers;

namespace Lib.Logs
{
    public class Log
    {
        class CommandOption
        {
            [Command("log.options.filename")] [CommandValue] public string FileName { get; set; } = @".\log.settings.json";
            public static CommandOption Load() => Arguments<CommandOption>.Load().Options;
        }
        class LogListenerLoader : ConfigLoader
        {
            public override object Load(Stream stream, Type type)
            {
                Listener.Reload(stream);
                Parameters.Clear();
                Listener.SelectMany(_ => _.CreateGenerators()).Foreach(_ => Parameters[_.keywords] = _.generator);
                Listener.Refresh();
                return new();
            }
        }
        static Log()
        {
            var option = CommandOption.Load();
            Config.Load<object>(option.FileName, new LogListenerLoader(), true);
        }
        public static LogListener Listener { get; } = new();
        public static LogParameters Parameters { get; } = new();
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
        public void Out() => Out(null);
        public void Out(object obj) => Out("{0}", obj);
        public void Out(string format, params object[] args)
        {
            var param = Parameters.Generate(this, string.Format(format, args));
            Listener.Where(_ => _.Level <= Level).Foreach(_ => _.Out(param));
        }
    }
}
