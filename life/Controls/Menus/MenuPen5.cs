using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public class MenuPen5 : MenuPen
    {
        readonly Tool _tool = Tools.Pen5;
        public override Tool Tool => _tool;
        public MenuPen5() : base() { }
        public MenuPen5(IContainer container) : base(container) { }
    }
}
