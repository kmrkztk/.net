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
            using var archive = ZipFile.Open(a[0], ZipArchiveMode.Update);
            foreach(var e in a.Skip(1))
            {
                if (Directory.Exists(e))
                {
                    var di = new DirectoryInfo(e);
                    var root = di.Parent.FullName + "\\";
                    di
                        .GetFiles("*.*", SearchOption.AllDirectories)
                        .Foreach(_ => Add(archive, _.FullName, _.FullName.Replace(root, "")));
                }
                else if (File.Exists(e)) Add(archive, e, new FileInfo(e).Name);
            }
        }
        static void View(string filepath)
        {
            var a = ZipFile.OpenRead(filepath);
            a.Entries.Foreach(_ => Console.WriteLine(_.FullName));
        }
        static void Add(ZipArchive a, string filepath, string entry) 
        {
            Console.WriteLine(entry);
            a.GetEntry(entry)?.Delete();
            a.CreateEntryFromFile(filepath, entry);
        }

        [Detail("zip (archive) [filename...]")]
        class Options
        {
            [Command] [Command("v")] public bool Viewing { get; set; }
            [Command] [Command("u")] public bool Updating { get; set; }
            [Command] [Command("w")] public bool OverWrite { get; set; }
            [Command] [Command("s")] public bool SubDirectory { get; set; }
        }
    }
}
