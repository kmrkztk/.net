using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public class MenuSaveAs : MenuFile
    {
        public override string Text { get => "SaveAs(&A)"; set => base.Text = value; }
        public override void Do() => Component?.SaveAs();
        public MenuSaveAs() : this(null) { }
        public MenuSaveAs(IContainer container) : base(container) { }
    }
}
