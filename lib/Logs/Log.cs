﻿using System;
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
                return new();
            }
        }
        static Log()
        {
            var option = CommandOption.Load();
            Try.Of(() => Config.Load<object>(option.FileName, new LogListenerLoader(), true)).Invoke();
            Listener.SelectMany(_ => _.CreateGenerators()).Do(_ => Parameters[_.keywords] = _.generator);
            Listener.Refresh();
        }
        public static LogListener Listener { get; } = new() { new TraceLogger(), };
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
        public void Out(string format, params object[] args) => Out(string.Format(format, args));
        public void Out(string value)
        {
            var param = Parameters.Generate(this, value);
            Listener.Where(_ => _.Level <= Level).Do(_ => _.Out(param));
        }
    }
}
