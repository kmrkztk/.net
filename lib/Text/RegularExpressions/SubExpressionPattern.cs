using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Text.RegularExpressions
{
    public class SubExpressionPattern : RegexPattern
    {
        readonly List<RegexPattern> _patterns = new();
        public RegexPattern this[int index] => _patterns[index];
        public override string Value =>
            MetaCharacters.BeginOfSubex + string.Join(MetaCharacters.SeparatorOfSubex, _patterns) + MetaCharacters.EndOfSubex;
        public SubExpressionPattern(params RegexPattern[] value) => _patterns.AddRange(value);
        SubExpressionPattern(SubExpressionPattern patterns, RegexPattern pattern) : this(patterns._patterns.ToArray()) => _patterns.Add(pattern);
        public SubExpressionPattern Append(RegexPattern pattern) => this | pattern;
        public static SubExpressionPattern operator |(SubExpressionPattern pattern1, RegexPattern pattern2) => new(pattern1, pattern2);
    }
}
