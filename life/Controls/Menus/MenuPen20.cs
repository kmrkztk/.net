using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public class MenuPen20 : MenuPen
    {
        readonly Tool _tool = Tools.Pen20; 
        public override Tool Tool => _tool;
        public MenuPen20() : base() { }
        public MenuPen20(IContainer container) : base(container) { }
    }
}
