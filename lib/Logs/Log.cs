using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Tracing;
using Lib.Jsons;
using Lib.Reflection;

namespace Lib.Logs
{
    public interface ILogger
    {
        void Out(Level level, string message);
    }
    public static class Log
    {
        static Log()
        {
            var loggers = typeof(ILogger).GetEnumerableOfType().ToDictionary(_ => NameAttribute.GetTypeName(_));
            var settings = Json.Load("log.settings.json");
            Listener = settings
                .AsArray()
                .Select(_ =>_.Cast(loggers[_["type"]?.Value ?? "default"]))
                .Cast<ILogger>()
                .ToList();
        }
        public static List<ILogger> Listener { get; }
        public static void Out(Level level, string format, params object[] args) => Listener.Foreach(_ => _.Out(level, string.Format(format, args)));
        public static void Out(Level level, object obj) => Out(level, "{0}", obj);
        public static void Trace(string format, params object[] args) => Out(Level.Trace, format, args);
        public static void Debug(string format, params object[] args) => Out(Level.Debug, format, args);
        public static void Info (string format, params object[] args) => Out(Level.Info , format, args);
        public static void Warn (string format, params object[] args) => Out(Level.Warn , format, args);
        public static void Error(string format, params object[] args) => Out(Level.Error, format, args);
        public static void Fatal(string format, params object[] args) => Out(Level.Fatal, format, args);
        public static void Trace(object obj) => Out(Level.Trace, obj);
        public static void Debug(object obj) => Out(Level.Debug, obj);
        public static void Info (object obj) => Out(Level.Info , obj);
        public static void Warn (object obj) => Out(Level.Warn , obj);
        public static void Error(object obj) => Out(Level.Error, obj);
        public static void Fatal(object obj) => Out(Level.Fatal, obj);
    }
}
