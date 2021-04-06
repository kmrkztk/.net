using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib;
using System.Windows.Forms;

namespace clipimage
{
    class Program
    {
        [STAThread]
        static void Main()
        {
            var a = Arguments.Load();
            var bmp = new Bitmap(a[0]);
            Clipboard.SetImage(bmp);
            //Clipboard.SetDataObject(bmp, true);
        }
    }
}
