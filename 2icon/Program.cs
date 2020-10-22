using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib;

namespace _2icon
{
    class Program
    {
        static void Main()
        {
            ConsoleEx.LoggingUnhandledException();
            var a = FileArguments.Load();
            if (a.Help) return;
            var bi = new FileInfo(a[0]);
            var ii = new FileInfo(bi.FullName.Replace(bi.Extension, ".ico"));
            using (var bmp = new Bitmap(a[0]))
            using (var ico = Icon.FromHandle(bmp.GetHicon()))
            using (var s = new FileStream(ii.FullName, FileMode.Create)) ico.Save(s);
        }
    }
}
