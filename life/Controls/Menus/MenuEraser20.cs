using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public class MenuEraser20 : MenuPen
    {
        readonly Tool _tool = Tools.Eraser20;
        public override Tool Tool => _tool;
        public MenuEraser20() : base() { }
        public MenuEraser20(IContainer container) : base(container) { }
    }
}
