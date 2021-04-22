using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Text.RegularExpressions
{
    public static class MetaCharacters
    {
        public const char BeginOfLine = '^';
        public const char EndOfLine = '$';
        public const char BeginOfBracket = '[';
        public const char EndOfBracket = ']';
        public const char BracketException = '^';
        public const char BracketRange = '-';
        public const char BeginOfSubex = '(';
        public const char EndOfSubex = ')';
        public const char SeparatorOfSubex = '|';
        public const char ZeroOrOne = '?';
        public const char ZeroOrMore = '*';
        public const char OneOrMore = '+';
        public const char Any = '.';
    }
}
