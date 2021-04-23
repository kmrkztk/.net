using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;
using Lib;
using Lib.Text.RegularExpressions;


namespace hoge
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                static void test(RegexPattern pattern, string input) => Console.WriteLine("'{0}' matches '{1}' : {2}", input, pattern, pattern.IsMatch(input));
                test(RegexPattern.OnlyLargeAlphabets    , "ABC"  );
                test(RegexPattern.OnlySmallAlphabets    , "abc"  );
                test(RegexPattern.OnlyAlphabets         , "Abc"  );
                test(RegexPattern.OnlyNumerics          , "123"  );
                test(RegexPattern.OnlyAlphaNumerics     , "Abc123"  );
                test(RegexPattern.OnlySymbols           , "!\"#$%&'()=~|-^\\@`[{]}:*;+/?\\_,<.>");
                test(RegexPattern.OnlyHalf              , "ABCdef123!\"#$%&'()=~|-^\\@`[{]}:*;+/?\\_,<.>");
                test(RegexPattern.OnlyFullLargeAlphabets, "ＡＢＣ"  );
                test(RegexPattern.OnlyFullSmallAlphabets, "ｂｃ"  );
                test(RegexPattern.OnlyFullAlphabets     , "Ａｂｃ"  );
                test(RegexPattern.OnlyFullNumerics      , "１２３４５"  );
                test(RegexPattern.OnlyFullAlphaNumerics , "ＡＢＣｄｆｇ１２３"  );
                test(RegexPattern.OnlyHalfKana          , "ｱｲｳｴｵｶﾞﾎﾟ"  );
                test(RegexPattern.OnlyFullKana          , "アイウエオガポ"  );
                test(RegexPattern.OnlyKana              , "アイエオｶｷｸｹｺｶﾞﾎﾟガポ"  );
                test(RegexPattern.OnlyFull              , "あいうえおＡＢＣアイウエオ"  );

                test(RegexPattern.Alphabets & RegexPattern.Numerics | RegexPattern.FullAlphaNumerics, "");
                test((RegexPattern.Alphabets & RegexPattern.Numerics & ".,").ZeroOrMore().InLine(), "1234.abcd,EFG");
                test(RegexPattern.Of("hoge") | "fuga" | "piyo", "hoge");

                test(RegexPattern.Numerics.Repete(3).Append("-").Append(RegexPattern.Numerics.Repete(3, 4)).InLine(), "1234-567890");
                test(RegexPattern.Numerics.Repete(3).Append("-").Append(RegexPattern.Numerics.Repete(3, 4)).InLine(), "123-6789");
            }
            finally
            {
#if DEBUG
                ConsoleEx.Pause();
#endif
            }
        }
    }
}
