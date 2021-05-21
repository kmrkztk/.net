using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.Logs.DefaultLoggers
{
    public abstract class Logger : ILogger
    {
        [ChainCaseName]
        public Level Level { get; init; }
#if DEBUG
            = Level.Debug;
#else
            = Level.Info;
#endif
        [LowerName] public string Format { get; init; } = @"{now:yyyy/MM/dd HH:mm:ss.fff}{sep}[{level,-5}]{sep}{thread:x}{sep}{category}{sep}{message}";
        [LowerName] public string Separator { get; init; } = ", ";
        string _format;
        public virtual void Initialize() => _format = Format
            .ReplaceKeywords("sep", Separator)
            .ReplaceKeywords(new[] { "now", "level", "thread", "category", "message", });
        public void Out(Level level, LogCaller caller, string message)
        {
            if (level < Level) return;
            Out(_format, Now, level, ThreadId, caller, message);
        }
        protected abstract void Out(string format, params object[] args);
        protected virtual DateTime Now => DateTime.Now;
        protected virtual int? ThreadId => Thread.CurrentThread.ManagedThreadId;
    }

}
