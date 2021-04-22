using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Text.RegularExpressions
{
    public class BracketPattern : RegexPattern
    {
        public override string Value => MetaCharacters.BeginOfBracket + base.Value + MetaCharacters.EndOfBracket;
        public BracketPattern(string value) : base(value) { }
        public BracketPattern(string from, string to) : this(from + MetaCharacters.BracketRange + to) { }
        public BracketPattern Except() => new(MetaCharacters.BracketException + Value);
        public static BracketPattern operator +(BracketPattern pattern1, BracketPattern pattern2) => new(pattern1._value + pattern2._value);
        public static BracketPattern operator !(BracketPattern pattern) => pattern.Except();
    }
}
