using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lib.Text;

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
        public static string UnicodeUnescape(this string value) => Regex.Replace(value, @"\\u[0-9a-fA-F]{4}", _ =>
        {
            var c = _.Value[2..].ToBinary();
            return Encoding.BigEndianUnicode.GetString(c.Part(c[0] == 0 ? 1 : 0));
        });
        public static string TrimEnd(this string value, string last) => value?.EndsWith(last) ?? false ? value.Remove(value.Length - last.Length) : value;
        public static string ToPascalCase(this string value)    => NameStyleCase.ToPascalCase(value);
        public static string ToCamelCase(this string value)     => NameStyleCase.ToCamelCase(value);
        public static string ToSnakeCase(this string value)     => NameStyleCase.ToSnakeCase(value);
        public static string ToConstantCase(this string value)  => NameStyleCase.ToConstantCase(value);
        public static string ToChainCase(this string value)     => NameStyleCase.ToChainCase(value);
        public static string ToKebabuCase(this string value)    => NameStyleCase.ToKebabuCase(value);
    }
}
