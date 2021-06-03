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
        [LowerName] public string Format { get; init; } = @"{timestamp:yyyy/MM/dd HH:mm:ss.fff}{sep}[{level,-5}]{sep}{thread:x}{sep}{method}{sep}{message}";
        [LowerName] public string Separator { get; init; } = ", ";
        string _format;
        public virtual void Dispose() => GC.SuppressFinalize(this);
        public virtual void Refresh() => _format = Format
            .ReplaceKeywords("sep", Separator)
            .ReplaceKeywords(Log.Parameters.Keys);
        public (string, LogParameters.Generator)[] CreateGenerators() => new (string, LogParameters.Generator)[]
        {
            ("level"        , (log, msg) => log.Level                               ),
            ("method"       , (log, msg) => log.Caller.MemberName                   ),
            ("linenumber"   , (log, msg) => log.Caller.LineNumber                   ),
            ("filepath"     , (log, msg) => log.Caller.FilePath                     ),
            ("filename"     , (log, msg) => new FileInfo(log.Caller.FilePath).Name  ),
            ("message"      , (log, msg) => msg                                     ),
            ("timestamp"    , (log, msg) => DateTime.Now                            ),
            ("thread"       , (log, msg) => Thread.CurrentThread.ManagedThreadId    ),
        };
        public void Out(object[] parameters)
        {
            try
            {
                Out(string.Format(_format, parameters));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }
        protected abstract void Out(string message);
    }
}
