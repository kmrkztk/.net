using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace life.IO
{
    public class MapFileCells : IMapFileExtension
    {
        public string Extension => ".cells";
        public const string CommentChar = "!";
        public const string NameKey = "Name: ";
        public const string AuthorKey = "Author: ";
        public const string CommentKey = "";
        public string Format(MapFileInfo info)
        {
            IEnumerable<string> format()
            {
                if (!string.IsNullOrEmpty(info.Name)) yield return string.Format("{0}{1}{2}\r\n", CommentChar, NameKey, info.Name);
                if (!string.IsNullOrEmpty(info.Author)) yield return string.Format("{0}{1}{2}\r\n", CommentChar, AuthorKey, info.Author);
                foreach (var c in info.Comments) yield return string.Format("{0}{1}{2}\r\n", CommentChar, CommentKey, c);
            }
            return string.Concat(format());
        }
        public string Format(Map map)
        {
            var sb = new StringBuilder();
            for (var y = 0; y < map.Height; y++)
            {
                var buf = "";
                for (var x = 0; x < map.Width; x++)
                {
                    if (map[x, y])
                    {
                        sb.Append(buf + 'O');
                        buf = "";
                    }
                    else buf += '.';
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
        public Map ReadMap(StreamReader reader)
        {
            var lines = ReadBody(reader).ToArray();
            var map = new Map(lines.Max(_ => _.Length), lines.Length);
            var x = 0;
            var y = 0;
            foreach (var l in lines)
            {
                foreach (var c in l)
                {
                    switch (c)
                    {
                        case '.': x++; break;
                        case 'O': map[x++, y] = true; break;
                    }
                }
                y++;
                x = 0;
            }
            return map;
        }
        public IEnumerable<string> ReadLines(StreamReader reader)
        {
            reader.DiscardBufferedData();
            reader.BaseStream.Seek(0, SeekOrigin.Begin);
            while (!reader.EndOfStream) yield return reader.ReadLine().Trim();
        }
        public IEnumerable<string> ReadHeads(StreamReader reader) => ReadLines(reader).TakeWhile(_ => _.StartsWith(CommentChar));
        public IEnumerable<string> ReadBody(StreamReader reader) => ReadLines(reader).SkipWhile(_ => _.StartsWith(CommentChar));
        IEnumerable<string> ReadComments(StreamReader reader, string key)
        {
            var head = string.Join("\n", ReadHeads(reader));
            var reg = new Regex(
                "(^" + CommentChar + NameKey + "(?<name>.*)$|" +
                "^" + CommentChar + AuthorKey + "(?<auth>.*)$|" +
                "^" + CommentChar + CommentKey + "?(?<comment>.*)$)",
                RegexOptions.IgnoreCase | RegexOptions.Multiline);
            return reg.Matches(head)
                .OfType<Match>()
                .Select(_ => _.Groups[key])
                .Where(_ => _.Success)
                .Select(_ => _.Value);
        }
        public string ReadName(StreamReader reader) => ReadComments(reader, "name").FirstOrDefault();
        public string ReadAuthor(StreamReader reader) => ReadComments(reader, "auth").FirstOrDefault();
        public string[] ReadComments(StreamReader reader) => ReadComments(reader, "comment").ToArray();
        public Size ReadSize(StreamReader reader)
        {
            var lines = ReadBody(reader).ToArray();
            return new Size(lines.Max(_ => _.Length), lines.Length);
        }
    }
}
