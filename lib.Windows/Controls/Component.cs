using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib.Windows.Gaming;

namespace Lib.Windows.Controls
{
    public partial class Component : System.ComponentModel.Component
    {
        public Component() : this(null) { }
        public Component(IContainer container)
        {
            container?.Add(this);
            InitializeComponent();
        }
        [Browsable(false)]
        public virtual IComponent Parent { get; set; }
        public override ISite Site 
        {
            get => base.Site;
            set
            {
                base.Site = value;
                Parent = Parent ?? (Site?.GetService(typeof(IDesignerHost)) as IDesignerHost)?.RootComponent;
            }
        }
    }
}
