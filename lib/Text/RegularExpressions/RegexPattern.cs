using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace lib.Text.RegularExpressions
{
    public class RegexPattern
    {
        public static RegexPattern Empty => new();
        public static RegexPattern Any => new(MetaCharacters.Any);
        public static BracketPattern LargeAlphabets => new("A", "Z");
        public static BracketPattern SmallAlphabets => new("a", "z");
        public static BracketPattern Alphabets => LargeAlphabets + SmallAlphabets;
        public static BracketPattern Numerics => new(@"\d");
        public static BracketPattern AlphaNumerics => Alphabets + Numerics;
        public static BracketPattern Symbols => 
            new BracketPattern(" ", "/") + 
            new BracketPattern(":", "@") +
            new BracketPattern(@"\[", "~");
        public static BracketPattern HalfChars => new(" ", "~");
        public static BracketPattern FullLargeAlphabets => new("Ａ", "Ｚ");
        public static BracketPattern FullSmallAlphabets => new("ａ", "ｚ");
        public static BracketPattern FullAlphabets => FullLargeAlphabets + FullSmallAlphabets;
        public static BracketPattern FullNumerics => new("０", "９");
        public static BracketPattern FullAlphaNumerics => FullAlphabets + FullNumerics;
        public static BracketPattern HalfKana => new("ｦ", "ﾟ");
        public static BracketPattern FullKana => new("ァ", "ヴ");
        public static BracketPattern Kana => HalfKana + FullKana;

        readonly protected string _value;
        public virtual string Value => _value;
        public RegexPattern() : this("") { }
        public RegexPattern(char value) => _value = new(value, 1);
        public RegexPattern(string value) => _value = value;

        public RegexPattern Append(string value) => new(Value + value);
        public RegexPattern Append(char value) => new(Value + value);
        public RegexPattern BeginOfLine() => new(MetaCharacters.BeginOfLine + Value);
        public RegexPattern EndOfLine() => new(Value + MetaCharacters.EndOfLine);
        public RegexPattern LineOf() => BeginOfLine().EndOfLine();
        public BracketPattern Bracket() => new(Value);
        public RegexPattern ZeroOrOne() => Append(MetaCharacters.ZeroOrOne);
        public RegexPattern ZeroOrMore() => Append(MetaCharacters.ZeroOrMore);
        public RegexPattern OneOrMore() => Append(MetaCharacters.OneOrMore);
        public RegexPattern Only() => Bracket().ZeroOrMore().LineOf();
        public RegexPattern Range(int length) => Append("{" + length + ",}");

        public override string ToString() => Value;
        public static implicit operator string(RegexPattern pattern) => pattern.Value;
        public static explicit operator RegexPattern(string pattern) => new(pattern);
        public static RegexPattern operator +(RegexPattern pattern1, RegexPattern pattern2) => new(pattern1.Value + pattern2.Value);
        public static SubExpressionPattern operator |(RegexPattern pattern1, RegexPattern pattern2) => new(pattern1, pattern2);

        public bool IsMatch(string input) => Regex.IsMatch(input, Value);
        public bool IsMatch(string input, RegexOptions options) => Regex.IsMatch(input, Value, options);
        public bool IsMatch(string input, RegexOptions options, TimeSpan matchTimeout) => Regex.IsMatch(input, Value, options, matchTimeout);
        public Match Match(string input, RegexOptions options) => Regex.Match(input, Value, options);
        public Match Match(string input) => Regex.Match(input, Value);
        public Match Match(string input, RegexOptions options, TimeSpan matchTimeout) => Regex.Match(input, Value, options, matchTimeout);
        public MatchCollection Matches(string input) => Regex.Matches(input, Value);
        public MatchCollection Matches(string input, RegexOptions options, TimeSpan matchTimeout) => Regex.Matches(input, Value, options, matchTimeout);
        public MatchCollection Matches(string input, RegexOptions options) => Regex.Matches(input, Value, options);
        public string Replace(string input, string replacement, RegexOptions options) => Regex.Replace(input, Value, replacement, options);
        public string Replace(string input, MatchEvaluator evaluator) => Regex.Replace(input, Value, evaluator);
        public string Replace(string input, MatchEvaluator evaluator, RegexOptions options) => Regex.Replace(input, Value, evaluator, options);
        public string Replace(string input, MatchEvaluator evaluator, RegexOptions options, TimeSpan matchTimeout) => Regex.Replace(input, Value, evaluator, options, matchTimeout);
        public string Replace(string input, string replacement, RegexOptions options, TimeSpan matchTimeout) => Regex.Replace(input, Value, replacement, options, matchTimeout);
        public string Replace(string input, string replacement) => Regex.Replace(input, Value, replacement);
        public string[] Split(string input, RegexOptions options, TimeSpan matchTimeout) => Regex.Split(input, Value, options, matchTimeout);
        public string[] Split(string input, RegexOptions options) => Regex.Split(input, Value, options);
        public string[] Split(string input) => Regex.Split(input, Value);
    }

}
