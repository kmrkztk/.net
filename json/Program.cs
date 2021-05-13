using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Lib;
using Lib.Jsons;

namespace json
{
    class Program
    {
        static void Main()
        {
            var a = FileArguments<Options>.Load();
            foreach (var json in a.GetReaders()
                .Select(_ =>
                {
                    if (!a.Options.Raw) _.Seek('{', '[');
                    return _;
                })
                .Select(_ => Json.Load(_)))
            {
                foreach (var j in json.Find(a.Options.Key.ToArray())) Console.WriteLine(j is JsonValue ? j.ToString() : j.Format(a.Options.Settings));
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
            [Command]
            [CommandValue]
            public string Keys { get => string.Join(".", Key); set => Key = value.Split(".").ToList(); }
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
            public JsonFormatSettings Settings => new()
            {
                Indent = Indent,
                IndentChar = IndentChar,
                LineSeparator = NoFormat ? null : "\r\n",
                Escape = !Unescape,
            };
        }
    }
}
