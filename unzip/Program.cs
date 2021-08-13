using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Compression;
using Lib;
using Lib.Logs;

namespace zip
{
    class Program
    {
        static void Main()
        {
            var a = Arguments<Options>.Load();
            if (a.Count() == 0)
            {
                a.PrintHelp();
                return;
            }
            Console.WriteLine("archive : {0}", a[0]);
            if (a.Options.Viewing)
            {
                View(a[0]);
                return;
            }
            var dst = a.Skip(1).FirstOrDefault() ?? ".\\";
            ZipFile.ExtractToDirectory(a[0], dst, a.Options.OverWrite);

        }
        static void View(string filepath)
        {
            var a = ZipFile.OpenRead(filepath);
            a.Entries.Foreach(_ => Console.WriteLine(_.FullName));
        }

        [Detail("unzip (archive) [destination]")]
        class Options
        {
            [Command] [Command("v")] public bool Viewing { get; set; }
            [Command] [Command("w")] public bool OverWrite { get; set; }
        }
    }
}
