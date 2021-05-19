using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Logs
{
    public class LogWriter : IDisposable
    {
        public static LogWriter Empty => new ();
        public static LogWriter OnConsole => new ConsoleLogWriter();
        public static LogWriter OnDebug => new DebugLogWriter();
        public static LogWriter ToFile(string filename) => new LogFileWriter(filename);

        protected LogWriter() { }
        public virtual long Length => 0;
        public virtual void Dispose() => GC.SuppressFinalize(this);
        public virtual void WriteLine(string format, params object[] args) { }

        class ConsoleLogWriter : LogWriter
        {
            public override void WriteLine(string format, params object[] args) => Console.WriteLine(format, args);
        }
        class DebugLogWriter : LogWriter
        {
            public override void WriteLine(string format, params object[] args) => System.Diagnostics.Trace.WriteLine(string.Format(format, args));
        }
        class LogFileWriter : LogWriter
        {
            readonly StreamWriter _writer;
            public override long Length => _writer.BaseStream.Length;
            public LogFileWriter(string path) => _writer = new StreamWriter(path, true);
            public override void Dispose()
            {
                base.Dispose();
                _writer.Dispose();
            }
            ~LogFileWriter() => _writer.Dispose();
            public override void WriteLine(string format, params object[] args) => _writer.WriteLine(format, args);
        }
    }
}
