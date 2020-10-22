using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public class MenuOpen : MenuFile
    {
        public override string Text { get => "Open(&O)"; set => base.Text = value; }
        public override void Do() => Component?.Open();
        public MenuOpen() : this(null) { }
        public MenuOpen(IContainer container) : base(container) { }
    }
}
