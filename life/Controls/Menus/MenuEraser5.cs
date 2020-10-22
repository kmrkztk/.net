using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public class MenuEraser5 : MenuPen
    {
        readonly Tool _tool = Tools.Eraser5;
        public override Tool Tool => _tool;
        public MenuEraser5() : base() { }
        public MenuEraser5(IContainer container) : base(container) { }
    }
}
