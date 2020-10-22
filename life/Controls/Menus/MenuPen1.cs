using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public class MenuPen1 : MenuPen
    {
        readonly Tool _tool = Tools.Pen1;
        public override Tool Tool => _tool;
        public MenuPen1() : base() { }
        public MenuPen1(IContainer container) : base(container) { }
    }
}
