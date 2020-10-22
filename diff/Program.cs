using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib;
using Lib.Diff.Algorithm;

namespace diff
{
    class Program
    {
        static void Main()
        {
            ConsoleEx.LoggingUnhandledException();
            var a = Arguments.Load<Options>();
            if (a.Help) return;
            if (a.Values.Count < 2) throw new ArgumentException();

            IEnumerable<string> read(string fn)
            {
                using (var f = new FileStream(fn, FileMode.Open))
                using (var r = new StreamReader(f))
                    while (!r.EndOfStream) yield return r.ReadLine();
            }

            var diff = read(a[0]).Diff(read(a[1]), a.Options.Algorithm);
            if (!a.Options.All) diff = diff.Where(_ => !_.Type.IsNone);
            if (a.Options.Delete) diff = diff.Where(_ => _.Type.IsDelete);
            if (a.Options.Insert) diff = diff.Where(_ => _.Type.IsInsert);

            var format = "{0}";
            if (!a.Options.Delete && !a.Options.Insert) format = "{2} " + format;
            if (a.Options.LineNumber) format = "{1," + (diff.Max(_ => _.Index).ToString().Length + 1) + "} " + format;

            foreach (var d in diff) Console.WriteLine(format, 
                d.Value,
                d.Index,
                d.Type.IsDelete ? '-' : d.Type.IsInsert ? '+' : ' ');
#if DEBUG
            ConsoleEx.Pause();
#endif
        }
        class Options
        {
            [Command] [Command("a")] public bool All { get; set; }
            [Command] [Command("n")] public bool LineNumber { get; set; }
            [Command] [Command("i")] public bool Insert { get; set; }
            [Command] [Command("d")] public bool Delete { get; set; }
            [Command] public bool DP { get; set; }
            [Command] public bool OND { get; set; }
            [Command] public bool ONP { get; set; }
            public DiffAlgorithm<string> Algorithm => 
                ONP ? DiffAlgorithm<string>.ONP : 
                OND ? DiffAlgorithm<string>.OND : 
                DP ? DiffAlgorithm<string>.DP : 
                DiffAlgorithm<string>.ONP;
        }
    }
}
