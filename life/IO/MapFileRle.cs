using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lib;

namespace life.IO
{
    public class MapFileRle : IMapFileExtension
    {
        public string Extension => ".rle";
        public const string CommentChar = "#";
        public const string NameKey = "N ";
        public const string AuthorKey = "O ";
        public const string CommentKey = "C ";
        public string Format(MapFileInfo info)
        {
            IEnumerable<string> format()
            {
                if (!string.IsNullOrEmpty(info.Name)) yield return string.Format("{0}{1}{2}{3}", CommentChar, NameKey, info.Name, Environment.NewLine);
                if (!string.IsNullOrEmpty(info.Author)) yield return string.Format("{0}{1}{2}{3}", CommentChar, AuthorKey, info.Author, Environment.NewLine);
                foreach (var c in info.Comments) yield return string.Format("{0}{1}{2}{3}", CommentChar, CommentKey, c, Environment.NewLine);
            }
            return string.Concat(format());
        }
        public string Format(Map map)
        {
            var sb = new StringBuilder();
            var head = string.Format("x = {0}, y = {1}, rule = B3/S23", map.Width, map.Height);
            for (var y = 0; y < map.Height; y++)
            {
                for (var x = 0; x < map.Width; x++) sb.Append(map[x, y] ? 'o' : 'b');
                sb.Append('$');
            }
            sb.Append('!');
            var buf = sb.ToString();
            buf = Regex.Replace(buf, @"b+\$!", @"!");
            buf = Regex.Replace(buf, @"\$b+\$", @"$$$$");
            buf = Regex.Replace(buf, @"(oo+|bb+|\$\$+)", _ => string.Format("{0}{1}", _.Value.Length, _.Value[0]));
            return head + Environment.NewLine + buf.SplitLine(100);
        }
        public Map ReadMap(StreamReader reader)
        {
            var map = new Map(ReadSize(reader));
            var lines = reader.ReadToEnd();
            lines = lines.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            var reg = new Regex(@"(?<count>\d+)?((?<alive>o)|(?<death>b)|(?<eol>\$)|(?<eof>!))", RegexOptions.IgnoreCase);
            var x = 0;
            var y = 0;
            foreach (var m in reg.Matches(lines).OfType<Match>())
            {
                var count = m.Groups["count"].Success ? int.Parse(m.Groups["count"].Value) : 1;
                if (!m.Success) break;
                else if (m.Groups["alive"].Success) foreach (var xi in Enumerable.Range(x, count)) map[x++, y] = true;
                else if (m.Groups["death"].Success) x += count;
                else if (m.Groups["eol"].Success) { y += count; x = 0; }
                else if (m.Groups["eof"].Success) break;
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
            var line = ReadBody(reader).First();
            var match = Regex.Match(line, @"^ *x *= *(?<width>\d+), *y *= *(?<height>\d+).*$", RegexOptions.IgnoreCase);
            return match.Success ? new Size(int.Parse(match.Groups["width"].Value), int.Parse(match.Groups["height"].Value)) : Size.Empty;
        }
    }
}
