using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public abstract class MenuPen : MenuEdit
    {
        public override string Text { get => string.Format("{0} x {1}", Tool.Map.Width, Tool.Map.Height); set => base.Text = value; }
        public MenuPen() : base() { }
        public MenuPen(IContainer container) : base(container) { }
    }
}
