using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public partial class MenuItem : ToolStripMenuItem
    {
        public MenuItem() : this(null) { }
        public MenuItem(IContainer container)
        {
            container?.Add(this);
            InitializeComponent();
            this.Click += (sender, e) => Do();
        }
        public virtual void Do() { }
    }
}
