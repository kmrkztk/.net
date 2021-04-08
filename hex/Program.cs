using Lib;
using Lib.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hex
{
    class Program
    {
        static void Main()
        {
            var a = FileArguments<Options>.Load();
            if (a.Options.Decode)
            {
                foreach (var r in a.GetReaders()) Console.WriteLine(Encoding.UTF8.GetString(r.ReadToEnd().ToBinary()));
            }
            else
            {
                if (a.Options.Binary)
                {
                    foreach (var f in a.GetFiles()) foreach (var h in File.ReadAllBytes(f.FullName).Split(a.Options.LineSize).Select(_ => _.ToHex())) Console.WriteLine(h);
                }
                else
                {
                    foreach (var r in a.GetReaders()) foreach (var h in Encoding.UTF8.GetBytes(r.ReadToEnd()).Split(a.Options.LineSize).Select(_ => _.ToHex())) Console.WriteLine(h);
                }
            }
        }
        class Options
        {
            [Command]
            [Command("z")]
            [CommandValue]
            [Detail("output line size.")]
            public int LineSize { get; set; } = -1;
            [Command]
            [Command("b")]
            [Detail("open file as binary.")]
            public bool Binary { get; set; }
            [Command]
            [Command("d")]
            [Detail("decode")]
            public bool Decode { get; set; } = false;
        }
    }
}
