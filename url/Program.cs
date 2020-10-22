using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Lib;

namespace url
{
    class Program
    {
        static void Main()
        {
            ConsoleEx.LoggingUnhandledException();
            var a = Arguments.Load<Options>();
            if (a.Help) return;
            foreach (var v in a.Values) Console.WriteLine(a.Options.Decode ? HttpUtility.UrlDecode(v) : HttpUtility.UrlEncode(v));
        }
        class Options
        {
            [Command]
            [Command("d")]
            public bool Decode { get; set; }
            [Command]
            [Command("a")]
            public bool Analyze { get; set; }
        }
    }
}
