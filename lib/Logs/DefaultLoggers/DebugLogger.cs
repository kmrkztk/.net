using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Lib.Logs.DefaultLoggers
{
    [Name("debug")] 
    public class DebugLogger : Logger 
    {
        protected override void Out(string format, params object[] args) => Debug.WriteLine(string.Format(format, args)); 
    }
}
