using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib;

namespace tail
{
    class Program
    {
        static void Main()
        {
            var a = FileArguments<Options>.Load();
            foreach (var reader in a.GetReaders().Select(_ => _ as StreamReader))
            {
                var stream = reader.BaseStream;
                if (stream.Length == 0) return;

                // 末尾が改行している場合はカウントしないように
                stream.Seek(-1, SeekOrigin.End);
                var last = stream.ReadByte();
                var count = a.Options.Count;
                if (last == 0x0a) count++;

                stream.SeekTo(SeekOrigin.End, 0x0a, count);
                Console.Write(reader.ReadToEnd());

                var pos = stream.Position;
                while (a.Options.Follow)
                {
                    System.Threading.Thread.Sleep(500);
                    if (pos > stream.Length)
                    {
                        stream.Position = stream.Length;
                        pos = stream.Position;
                    }
                    if (pos == stream.Length) continue;
                    Console.Write(reader.ReadToEnd());
                    pos = stream.Position;
                }
                break;
            }
        }

        [Detail("tail file [/options]")]
        class Options
        {
            [Command] [Command("n")] [CommandValue] public int Count { get; set; } = 10;
            [Command] [Command("f")] public bool Follow { get; set; } = false;
        }
    }
}
