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
            var a = Arguments<Options>.Load();
            if (a.Count < 2) a.GoHelp();
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
        }
        [Detail("diff file1 file2 [/options]")]
        [Detail("compare files line by line.")]
        class Options
        {
            [Command] [Command("a")] [Detail("output all lines.")]        public bool All { get; set; }
            [Command] [Command("n")] [Detail("output line number.")]      public bool LineNumber { get; set; }
            [Command] [Command("i")] [Detail("output only insert line.")] public bool Insert { get; set; }
            [Command] [Command("d")] [Detail("output only delete line.")] public bool Delete { get; set; }
            [Command] [Detail("run with 'DP' algorithm.")]  public bool DP  { get; set; }
            [Command] [Detail("run with 'OND' algorithm.")] public bool OND { get; set; }
            [Command] [Detail("run with 'ONP' algorithm.")] public bool ONP { get; set; }
            public DiffAlgorithm<string> Algorithm => 
                ONP ? DiffAlgorithm<string>.ONP : 
                OND ? DiffAlgorithm<string>.OND : 
                DP ? DiffAlgorithm<string>.DP : 
                DiffAlgorithm<string>.ONP;
        }
    }
}
