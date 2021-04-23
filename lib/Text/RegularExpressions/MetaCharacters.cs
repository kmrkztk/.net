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
        public const char Except = '^';
        public const char BracketRange = '-';
        public const char BeginOfSubex = '(';
        public const char EndOfSubex = ')';
        public const char SeparatorOfSubex = '|';
        public const char BeginOfRepetition = '{';
        public const char EndOfRepetition = '}';
        public const char ZeroOrOne = '?';
        public const char ZeroOrMore = '*';
        public const char OneOrMore = '+';
        public const char Any = '.';
        public const char Escape = '\\';
        public static char[] All => new[]
        {
            BeginOfLine, EndOfLine, BeginOfBracket, EndOfBracket, Except, BracketRange,
            BeginOfSubex, EndOfSubex, SeparatorOfSubex, BeginOfRepetition, EndOfRepetition,
            ZeroOrOne, ZeroOrMore, OneOrMore, Any, Escape,
        };

        public const string LineSeparator = @"\n";
        public const string Tab = @"\t";
        public const string Space = @"\s";
        public const string Digit = @"\d";
        public const string Word = @"\w";
        public const string NotSpace = @"\S";
        public const string NotDigit = @"\D";
        public const string NotWord = @"\W";
    }
}
