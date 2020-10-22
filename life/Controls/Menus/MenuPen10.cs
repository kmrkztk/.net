using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public class MenuPen10 : MenuPen
    {
        readonly Tool _tool = Tools.Pen10;
        public override Tool Tool => _tool;
        public MenuPen10() : base() { }
        public MenuPen10(IContainer container) : base(container) { }
    }
}
