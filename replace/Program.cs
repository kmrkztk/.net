using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib;

namespace replace
{
    class Program
    {
        static void Main()
        {
            ConsoleEx.LoggingUnhandledException();
            var a = FileArguments.Load<Options>();
            if (a.Help) return;
            foreach (var reader in a.GetReaders())
            {
                Console.WriteLine(reader.ReadToEnd().Replace(a.Options.Pattern, a.Options.Replace));
            }
        }
        class Options
        {
            [Command("p")]
            [CommandValue]
            [Detail("regex pattern.")]
            public string Pattern { get; set; }
            [Command("r")]
            [CommandValue]
            [Detail("regex replace /p pattern")]
            public string Replace { get; set; }
        }
    }
}
