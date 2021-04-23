using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Lib.Text.RegularExpressions
{
    public class RegexPattern
    {
        public static RegexPattern Empty                    => new();
        public static RegexPattern Of(string pattern)       => new(pattern);
        public static RegexPattern Of(char pattern)         => new(pattern);
        public static RegexPattern Of(string p1, string p2) => new BracketPattern(p1, p2);
        public static RegexPattern InBracket(string pattern) => new BracketPattern(pattern);
        public static RegexPattern Or(params string[] patterns) => new SubExpressionPattern(patterns);
        public static RegexPattern Any                      => Of(MetaCharacters.Any);
        public static RegexPattern LargeAlphabets           => Of("A", "Z");
        public static RegexPattern SmallAlphabets           => Of("a", "z");
        public static RegexPattern Alphabets                => LargeAlphabets & SmallAlphabets;
        public static RegexPattern Numerics                 => Of(@"\d");
        public static RegexPattern AlphaNumerics            => Alphabets & Numerics;
        public static RegexPattern Symbols                  => Of(" ", "/") & Of(":", "@") & Of(@"\[", "~");
        public static RegexPattern Half                     => Of(" ", "~");
        public static RegexPattern FullLargeAlphabets       => Of("Ａ", "Ｚ");
        public static RegexPattern FullSmallAlphabets       => Of("ａ", "ｚ");
        public static RegexPattern FullAlphabets            => FullLargeAlphabets & FullSmallAlphabets;
        public static RegexPattern FullNumerics             => Of("０", "９");
        public static RegexPattern FullAlphaNumerics        => FullAlphabets & FullNumerics;
        public static RegexPattern HalfKana                 => Of("ｦ", "ﾟ");
        public static RegexPattern FullKana                 => Of("ァ", "ヴ");
        public static RegexPattern Kana                     => HalfKana & FullKana;
        public static RegexPattern Full                     => (Half & HalfKana).Except();
        public static RegexPattern OnlyLargeAlphabets       => LargeAlphabets     .ZeroOrMore().InLine();
        public static RegexPattern OnlySmallAlphabets       => SmallAlphabets     .ZeroOrMore().InLine();
        public static RegexPattern OnlyAlphabets            => Alphabets          .ZeroOrMore().InLine();
        public static RegexPattern OnlyNumerics             => Numerics           .ZeroOrMore().InLine();
        public static RegexPattern OnlyAlphaNumerics        => AlphaNumerics      .ZeroOrMore().InLine();
        public static RegexPattern OnlySymbols              => Symbols            .ZeroOrMore().InLine();
        public static RegexPattern OnlyHalf                 => Half               .ZeroOrMore().InLine();
        public static RegexPattern OnlyFullLargeAlphabets   => FullLargeAlphabets .ZeroOrMore().InLine();
        public static RegexPattern OnlyFullSmallAlphabets   => FullSmallAlphabets .ZeroOrMore().InLine();
        public static RegexPattern OnlyFullAlphabets        => FullAlphabets      .ZeroOrMore().InLine();
        public static RegexPattern OnlyFullNumerics         => FullNumerics       .ZeroOrMore().InLine();
        public static RegexPattern OnlyFullAlphaNumerics    => FullAlphaNumerics  .ZeroOrMore().InLine();
        public static RegexPattern OnlyHalfKana             => HalfKana           .ZeroOrMore().InLine();
        public static RegexPattern OnlyFullKana             => FullKana           .ZeroOrMore().InLine();
        public static RegexPattern OnlyKana                 => Kana               .ZeroOrMore().InLine();
        public static RegexPattern OnlyFull                 => Full               .ZeroOrMore().InLine();

        readonly protected string _value;
        public virtual string Value => _value;
        public RegexPattern() : this("") { }
        public RegexPattern(char value) => _value = new(value, 1);
        public RegexPattern(string value) => _value = value;

        public virtual RegexPattern Append(RegexPattern value)  => new(Value + value.Value);
        public virtual RegexPattern Append(string value)        => new(Value + value);
        public virtual RegexPattern Append(char value)          => new(Value + value);
        public virtual RegexPattern AndAny()                    => new(Value + MetaCharacters.Any);
        public virtual RegexPattern ZeroOrOne()                 => Append(MetaCharacters.ZeroOrOne);
        public virtual RegexPattern ZeroOrMore()                => Append(MetaCharacters.ZeroOrMore);
        public virtual RegexPattern OneOrMore()                 => Append(MetaCharacters.OneOrMore);
        public virtual RegexPattern Repete(int length)          => Append(string.Format("{0}{1}{2}", MetaCharacters.BeginOfRepetition, length, MetaCharacters.EndOfRepetition));
        public virtual RegexPattern Repete(int? min, int? max)  => Append(string.Format("{0}{1},{2}{3}", MetaCharacters.BeginOfRepetition, min, max, MetaCharacters.EndOfRepetition));
        public virtual RegexPattern BeginOfLine()               => new(MetaCharacters.BeginOfLine + Value);
        public virtual RegexPattern EndOfLine()                 => new(Value + MetaCharacters.EndOfLine);
        public virtual RegexPattern InLine()                    => BeginOfLine().EndOfLine();
        public virtual RegexPattern InBracket()                 => InBracket(Value);
        public virtual RegexPattern Union(RegexPattern value)   => InBracket(_value + value._value);
        public virtual RegexPattern Except()                    => InBracket(MetaCharacters.Except + _value);
        public virtual RegexPattern Or(RegexPattern pattern)    => new SubExpressionPattern(this, pattern);
        public virtual RegexPattern Escape()
        {
            var v = (string)Value.Clone();
            var esc = MetaCharacters.Escape;
            v = v.Replace(esc.ToString(), new string(esc, 2));
            foreach (var c in MetaCharacters.All.Where(_ => _ != esc).Select(_ => _.ToString())) v = v.Replace(c, esc + c);
            return new(v);
        }

        public override string ToString() => Value;
        public static implicit operator string(RegexPattern pattern) => pattern.Value;
        public static explicit operator RegexPattern(string pattern) => new(pattern);
        public static RegexPattern operator +(RegexPattern pattern1, RegexPattern pattern2) => pattern1.Append(pattern2);
        public static RegexPattern operator +(RegexPattern pattern1, string pattern2)       => pattern1.Append(pattern2);
        public static RegexPattern operator +(RegexPattern pattern1, char pattern2)         => pattern1.Append(pattern2);
        public static RegexPattern operator &(RegexPattern pattern1, RegexPattern pattern2) => pattern1.Union(pattern2);
        public static RegexPattern operator &(RegexPattern pattern1, string pattern2)       => pattern1.Union(Of(pattern2));
        public static RegexPattern operator &(RegexPattern pattern1, char pattern2)         => pattern1.Union(Of(pattern2));
        public static RegexPattern operator |(RegexPattern pattern1, RegexPattern pattern2) => pattern1.Or(pattern2);
        public static RegexPattern operator |(RegexPattern pattern1, string pattern2)       => pattern1.Or(Of(pattern2));
        public static RegexPattern operator |(RegexPattern pattern1, char pattern2)         => pattern1.Or(Of(pattern2));

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
        /// <summary> [...] </summary>
        class BracketPattern : RegexPattern
        {
            public override string Value => MetaCharacters.BeginOfBracket + base.Value + MetaCharacters.EndOfBracket;
            public BracketPattern(string value) : base(value) { }
            public BracketPattern(string from, string to) : this(from + MetaCharacters.BracketRange + to) { }
        }
        /// <summary> (...) </summary>
        class SubExpressionPattern : RegexPattern
        {
            readonly List<RegexPattern> _patterns = new();
            public RegexPattern this[int index] => _patterns[index];
            public override string Value =>
                MetaCharacters.BeginOfSubex + string.Join(MetaCharacters.SeparatorOfSubex, _patterns) + MetaCharacters.EndOfSubex;
            public SubExpressionPattern(params string[] value) => _patterns.AddRange(value.Select(_ => Of(_)));
            public SubExpressionPattern(params RegexPattern[] value) => _patterns.AddRange(value);
            public SubExpressionPattern(SubExpressionPattern patterns, params RegexPattern[] pattern) : this(patterns._patterns.Concat(pattern).ToArray()) { }
            public override RegexPattern Or(RegexPattern pattern) => new SubExpressionPattern(this, pattern);
        }
    }
}
