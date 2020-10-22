using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lib;

namespace grep
{
    class Program
    {
        static void Main()
        {
            ConsoleEx.LoggingUnhandledException();
            var a = FileArguments.Load<Options>();
            if (a.Help) return;
            var r = a.Options.Regex;
            var startwith = a.Options.StartWithRegex;
            var format = "{0}";
            if (a.Options.DisplayFileName) format = "{1}({2}) :" + format;
            foreach (var reader in a.GetReaders())
            {
                string line;
                int take = 0;
                int count = 0;
                string name = "unknown";
                bool start = false;
                if (Console.IsInputRedirected) name = "pipe";
                if (a.Options.DisplayFileName && reader is StreamReader sr && sr.BaseStream is FileStream fs) name = fs.Name;
                while ((line = reader.ReadLine()) != null)
                {
                    start = start || startwith.IsMatch(line);
                    if (start &&
                    ++count > a.Options.Skip
                    && take++ < a.Options.Take
                    && r.IsMatch(line)) Console.WriteLine(format, line, name, count);
                }
            }
        }
        [Detail("grep text in files.")]
        class Options
        {
            public Regex Regex =>
                new Regex(
                    Debug ? @"(\[FATAL\]|\[ERROR\]|\[INFO \]|\[DEBUG\])" :
                    Info  ? @"(\[FATAL\]|\[ERROR\]|\[INFO \])" :
                    Error ? @"(\[FATAL\]|\[ERROR\])" :
                    Fatal ? @"(\[FATAL\])" : Pattern, RegexOption);
            public Regex StartWithRegex => new Regex("^" + StartWith + ".*");
            [Command]
            [Detail("equal /p \"(\\[FATAL\\])\"")]
            public bool Fatal { get; set; }
            [Command]
            [Detail("equal /p \"(\\[FATAL\\]|\\[ERROR\\])\"")]
            public bool Error { get; set; }
            [Command]
            [Detail("equal /p \"(\\[FATAL\\]|\\[ERROR\\]|\\[INFO \\])\"")]
            public bool Info { get; set; }
            [Command]
            [Detail("equal /p \"(\\[FATAL\\]|\\[ERROR\\]|\\[INFO \\]|\\[DEBUG\\])\"")]
            public bool Debug { get; set; }
            [Command]
            [Command("p")]
            [CommandValue]
            [Detail("regex pattern.")]
            public string Pattern { get; set; } = ".*";
            [Command]
            [CommandValue]
            [Detail("skip head n lines.")]
            public int Skip { get; set; } = 0;
            [Command]
            [CommandValue]
            [Detail("take n lines.")]
            public int Take { get; set; } = int.MaxValue;
            [Command]
            [Command("f")]
            [Detail("display filename.")]
            public bool DisplayFileName { get; set; }
            [Command]
            [CommandValue]
            [Detail("start with (char).")]
            public string StartWith { get; set; }
            [Command]
            [Detail("not ignore case.")]
            public bool CheckCase { get; set; }
            public RegexOptions RegexOption => CheckCase ? RegexOptions.None : RegexOptions.IgnoreCase;
        }
    }
}
