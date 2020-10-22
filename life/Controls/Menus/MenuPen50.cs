using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public class MenuPen50 : MenuPen
    {
        readonly Tool _tool = Tools.Pen50;
        public override Tool Tool => _tool;
        public MenuPen50() : base() { }
        public MenuPen50(IContainer container) : base(container) { }
    }
}
