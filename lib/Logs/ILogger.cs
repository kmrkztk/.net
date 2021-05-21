using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Logs
{
    public interface ILogger
    {
        void Initialize();
        void Out(Level level, LogCaller caller, string message);
    }
}
