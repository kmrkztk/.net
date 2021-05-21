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
        public static string ReplaceKeywords(this string input, string key, string parameter) => ReplaceKeywords(input, new[] { key }, new[] { parameter });
        public static string ReplaceKeywords(this string input, string[] keys, string[] parameters) => ReplaceKeywords(input, RegexOptions.IgnoreCase, keys, parameters);
        public static string ReplaceKeywords(this string input, RegexOptions option, string[] keys, string[] parameters)
        {
            for (var i = 0; i < keys.Length; i++) input = Regex.Replace(input, "{" + keys[i] + "([^}]*)}", parameters[i], option);
            return input;
        }
        public static string ReplaceKeywords(this string input, string key) => ReplaceKeywords(input, new[] { key });
        public static string ReplaceKeywords(this string input, string[] keys) => ReplaceKeywords(input, RegexOptions.IgnoreCase, keys);
        public static string ReplaceKeywords(this string input, RegexOptions option, string[] keys) => ReplaceKeywords(input, option, keys, keys.Select((_, i) => "{" + i + "$1}").ToArray());
        public static string Format(this string input, params object[] parameters) => string.Format(input, parameters);
        public static string TrimEnd(this string value, string last) => value?.EndsWith(last) ?? false ? value.Remove(value.Length - last.Length) : value;
        public static string ToPascalCase(this string value)    => NameStyleCase.ToPascalCase(value);
        public static string ToCamelCase(this string value)     => NameStyleCase.ToCamelCase(value);
        public static string ToSnakeCase(this string value)     => NameStyleCase.ToSnakeCase(value);
        public static string ToConstantCase(this string value)  => NameStyleCase.ToConstantCase(value);
        public static string ToChainCase(this string value)     => NameStyleCase.ToChainCase(value);
        public static string ToKebabuCase(this string value)    => NameStyleCase.ToKebabuCase(value);
    }
}
