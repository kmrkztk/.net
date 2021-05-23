using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Logs.DefaultLoggers
{
    [Name("console")]
    public class ConsoleLogger : Logger
    {
        protected override void Out(string message) => Console.WriteLine(message);
    }
}
