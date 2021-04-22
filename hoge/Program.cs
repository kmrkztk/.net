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
                Console.WriteLine(RegexPattern.OnlyAlphabets);
                Console.WriteLine(RegexPattern.Alphabets + RegexPattern.Numerics + RegexPattern.FullAlphabets + RegexPattern.FullNumerics);
                Console.WriteLine(RegexPattern.OnlyAlphabets | RegexPattern.OnlyKana);
                
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
