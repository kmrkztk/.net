using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Lib;
using Lib.Json;

namespace json
{
    class Program
    {
        static void Main()
        {
            var a = FileArguments.Load<Options>();
            foreach (var j in a.GetReaders()
                .Select(_ =>
                {
                    if (!a.Options.Raw) _.Seek('{');
                    return _;
                })
                .Select(_ => Json.Load(_)))
            {
                if (a.Options.HasKey)
                {
                    foreach (var v in j.Find(a.Options.Key.ToArray())) Console.WriteLine(v is JsonValue ? v.ToString() : v.Format(a.Options.Settings));
                }
                else
                {
                    Console.WriteLine(j.Format(a.Options.Settings));
                }
            }
#if DEBUG
            ConsoleEx.Pause();
#endif
        }
        class Options
        {
            [Command]
            [Command("k")]
            [CommandValue]
            public List<string> Key { get; set; } = new List<string>();
            public bool HasKey => Key.Count > 0;
            [Command("indent-char")]
            [CommandValue]
            public char IndentChar { get; set; } = ' ';
            [Command]
            [CommandValue]
            public int Indent { get; set; } = 2;
            [Command]
            public bool Unescape { get; set; } = true;
            [Command]
            public bool Escape { get => !Unescape; set => Unescape = !value; }
            [Command("none")]
            public bool NoFormat { get; set; }
            [Command]
            public bool Raw { get; set; }
            public JsonFormatSettings Settings => new JsonFormatSettings()
            {
                Indent = Indent,
                IndentChar = IndentChar,
                LineSeparator = NoFormat ? null : "\r\n",
                Escape = !Unescape,
            };
        }
    }
}
