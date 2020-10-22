using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lib;

namespace uni
{
    class Program
    {
        static void Main()
        {
            ConsoleEx.LoggingUnhandledException();
            var a = FileArguments.Load<Options>();
            if (a.Help) return;
            foreach (var l in a.GetLines()) Console.WriteLine(a.Options.Encode ? l.UnicodeEscape(a.Options.Forced) : Regex.Unescape(l));
        }
        class Options
        {
            [Command("encode")]
            [Command("e")]
            public bool Encode { get; set; } = false;
            [Command("forced")]
            public bool Forced { get; set; } = false;
        }
    }
}
