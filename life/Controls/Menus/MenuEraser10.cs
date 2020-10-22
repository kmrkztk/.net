using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public class MenuEraser10 : MenuPen
    {
        readonly Tool _tool = Tools.Eraser10;
        public override Tool Tool => _tool;
        public MenuEraser10() : base() { }
        public MenuEraser10(IContainer container) : base(container) { }
    }
}
