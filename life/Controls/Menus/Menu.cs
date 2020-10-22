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
    public partial class Menu : MenuStrip
    {
        public Menu() : this(null) { }
        public Menu(IContainer container)
        {
            container?.Add(this);
            InitializeComponent();
        }
    }
}
