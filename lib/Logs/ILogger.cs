using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Logs
{
    public interface ILogger : IDisposable
    {
        Level Level { get; }
        (string keywords, LogParameters.Generator generator)[] CreateGenerators();
        void Refresh();
        void Out(object[] parameters);
    }
}
