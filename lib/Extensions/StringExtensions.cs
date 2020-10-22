using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lib
{
    public static class StringExtensions
    {
        public static IEnumerable<string> Split(this string value, int count) {
            var length = value.Length;
            var offset = 0;
            while (length > 0)
            {
                yield return value.Substring(offset, length > count ? count : length);
                length -= count;
                offset += count;
            }
        }
        public static string SplitLine(this string value, int count) => SplitLine(value, count, Environment.NewLine);
        public static string SplitLine(this string value, int count, string separator) => count > 0 ? Regex.Replace(value, @"(?<=\G.{" + count + "})(?!$)", separator) : value;
        public static string UnicodeEscape(this string value, bool force = false) => Regex.Replace(value, force ? "." : "[^\x00-\x7F]", _ => string.Format(@"\u{0:x4}", (int)_.Value[0]));
        public static string UnicodeUnescape(this string value) => Regex.Replace(value, @"\\u.{4}", _ =>
        {
            var c = _.Value.Substring(2).ToBinary();
            return Encoding.UTF8.GetString(c.Part(c[0] == 0 ? 1 : 0));
        });
    }
}
