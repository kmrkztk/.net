using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Logs
{
    public class LogCaller
    {
        public string MemberName { get; init; }
        public string FilePath { get; init; }
        public int LineNumber { get; init; }
        public override string ToString() => MemberName;
    }
}
