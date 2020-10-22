using System;
using System.Drawing;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib;
using Lib.Windows.Drawing;
using Lib.Bit;
using Lib.Windows.Controls;
using Lib.Windows.Controls.Design;
using life.IO;
using life.Controls;
using System.Reflection;

namespace hoge
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                var types = Lib.Reflection.ReflectiveEnumerator.GetEnumerableOfType<Lib.Windows.Gaming.Drawer>();
                foreach (var t in types) Console.WriteLine(t);

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
