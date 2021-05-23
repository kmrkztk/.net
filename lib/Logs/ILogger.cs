using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Logs
{
    public interface ILogger
    {
        Level Level { get; }
        (string keywords, LogParameters.Generator generator)[] CreateGenerators();
        void Initialize();
        void Out(object[] parameters);
    }
}
