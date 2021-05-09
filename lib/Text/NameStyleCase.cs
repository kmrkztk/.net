using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;

namespace Lib.Text
{
    public static class NameStyleCase
    {
        public static string[] Normalize(string value) => typeof(NameStyleCase)
            .GetProperties(BindingFlags.Public | BindingFlags.Static)
            .Select(_ => _.GetValue(null))
            .Cast<Rule>()
            .FirstOrDefault(_ => _.IsMatch(value))
            ?.Normalize(value)
            ?? new[] { value };
        public static Rule PascalCase   => new PascalCaseRule();
        public static Rule CamelCase    => new CamelCaseRule();
        public static Rule SnakeCase    => new SnakeCaseRule();
        public static Rule ConstantCase => new ConstantCaseRule();
        public static Rule ChainCase    => new ChainCaseRule();
        public static Rule KebabuCase   => new KebabuCaseRule();
        public static string ToPascalCase(string value)     => PascalCase.Format(Normalize(value));
        public static string ToCamelCase(string value)      => CamelCase.Format(Normalize(value));
        public static string ToSnakeCase(string value)      => SnakeCase.Format(Normalize(value));
        public static string ToConstantCase(string value)   => ConstantCase.Format(Normalize(value));
        public static string ToChainCase(string value)      => ChainCase.Format(Normalize(value));
        public static string ToKebabuCase(string value)     => KebabuCase.Format(Normalize(value));

        public abstract class Rule
        {
            public abstract string Separator { get; }
            public abstract string Pattern { get; }
            public abstract string Format(string value, int index);
            public virtual string Format(string[] words) => string.Join(Separator, words.Select((_, i) => Format(_, i)));
            public virtual string[] Normalize(string value) =>
                !string.IsNullOrEmpty(Separator) ? value?.Split(Separator) :
                Regex.Match(value, Pattern).Groups.Values
                .Skip(1)
                .SelectMany(_ => _.Captures.Select(_ => _.Value))
                .ToArray();
            public bool IsMatch(string value) => Regex.IsMatch(value, Pattern);
        }
        class PascalCaseRule : Rule
        {
            public override string Separator => "";
            public override string Pattern => "^([A-Z][a-z]*|[0-9]+[a-z]*)*$";
            public override string Format(string value, int index) =>
                string.IsNullOrEmpty(value) ? value :
                value.Length == 1 ? value.ToUpper() :
                value.Substring(0, 1).ToUpper() + value[1..].ToLower();
        }
        class CamelCaseRule : PascalCaseRule
        {
            public override string Pattern => "^([a-z]+)([A-Z][a-z]*|[0-9]+[a-z]*)*$";
            public override string Format(string value, int index) => 
                index == 0 ? value?.ToLower() : base.Format(value, index);
        }
        class SnakeCaseRule : Rule
        {
            public override string Separator => "_";
            public override string Pattern => "^[a-zA-Z]+(_[0-9a-zA-Z]+)*$";
            public override string Format(string value, int index) => value?.ToLower();
        }
        class ConstantCaseRule : SnakeCaseRule
        {
            public override string Format(string value, int index) => value?.ToUpper();
        }
        class ChainCaseRule : Rule
        {
            public override string Separator => "-";
            public override string Pattern => "^[a-zA-Z]+(-[0-9a-zA-Z]+)*$";
            public override string Format(string value, int index) => value?.ToLower();
        }
        class KebabuCaseRule : ChainCaseRule
        {
            public override string Format(string value, int index) =>
                index > 0 ? value?.ToLower() :
                string.IsNullOrEmpty(value) ? value :
                value.Length == 1 ? value.ToUpper() :
                value.Substring(0, 1).ToUpper() + value[1..].ToLower();
        }
    }
}
