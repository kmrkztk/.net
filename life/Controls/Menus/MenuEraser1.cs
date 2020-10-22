using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public class MenuEraser1 : MenuPen
    {
        readonly Tool _tool = Tools.Eraser1;
        public override Tool Tool => _tool;
        public MenuEraser1() : base() { }
        public MenuEraser1(IContainer container) : base(container) { }
    }
}
