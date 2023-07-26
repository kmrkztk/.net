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
            var key = a.Options.Key.ToArray();
            var setting = a.Options.Settings;
            a   .GetReaders()
                .Each(_ => !a.Options.Raw, _ => _.Seek('{', '['))
                .Select(_ => Json.Load(_))
                .SelectMany(_ => _.Find(key))
                .Console(_ => _ is JsonValue ? _.ToString() : _.Format(setting))
                .Do();
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
            [Command]
            [Command("w")]
            [CommandValue]
            public Dictionary<string, string> Where { get; set; }
            [Command("nested")]
            public bool NestedKeys { get; set; }

            // format
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

            public bool HasKey => Key.Count > 0;
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
