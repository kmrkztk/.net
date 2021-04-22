using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RegexMatchValidationAttribute : ValidationAttribute
    {
        public const string LargeAlphabetPattern = @"\u";
        public const string SmallAlphabetPattern = @"\l";
        public const string AlphabetPattern = LargeAlphabetPattern + SmallAlphabetPattern;
        public const string NumericPattern = @"\d";
        public const string AlphaNumericPattern = AlphabetPattern + NumericPattern;
        public const string SymbolPattern = @" -/:-@\[-~";

        public const string HalfKanaPattern = @"ｦ-ﾟ";
        public const string FullKanaPattern = @"ァ-ヴ";
        public const string KanaPattern = HalfKanaPattern + FullKanaPattern;
        
        public const string FullLargeAlphabetPattern = "Ａ-Ｚ";
        public const string FullSmallAlphabetPattern = "ａ-ｚ";
        public const string FullAlphabetPattern = FullLargeAlphabetPattern + FullSmallAlphabetPattern;
        public const string FullNumericPattern = "０-９";
        public const string FullAlphaNumericPattern = FullAlphabetPattern + FullNumericPattern;

        public static string Bracket(string pattern) => "[" + pattern + "]";
        public static string ZeroOrOne(string pattern) => pattern + "?";
        public static string ZeroOrMore(string pattern) => pattern + "*";
        public static string OneOrMore(string pattern) => pattern + "+";
        public static string BeginOfLine(string pattern) => "^" + pattern;
        public static string EndOfLine(string pattern) => pattern + "$";
        public static string OfLine(string pattern) => BeginOfLine(EndOfLine(pattern));
        public static string Only(string pattern) => OfLine(ZeroOrMore(Bracket(pattern)));

        public override string DefaultMessage => "'{name}' dose not match the pattern '{pattern}'";
        public string Pattern { get; init; }
        public RegexOptions Options { get; init; } = RegexOptions.None;
        public RegexMatchValidationAttribute(string pattern) => Pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
        public override string GetMessage(Property property, ValidationContext context) => base.GetMessage(property, context).Replace("{pattern}", Pattern);
        public override bool HasError(Property property, ValidationContext context) => !Regex.IsMatch((string)property.GetValue() ?? "", Pattern, Options);
    }
}
