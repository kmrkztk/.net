using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Lib.Logs.DefaultLoggers
{
    [Name("trace")]
    public class TraceLogger : Logger
    {
        protected override void Out(string message) => Trace.WriteLine(message);
        public TraceLogger() : base() { Level = Level.Trace; }
    }
}
