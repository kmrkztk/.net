using Lib.Windows.Controls.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lib.Windows.Controls
{
    public partial class Trigger : Component
    {
        public Trigger() : this(null) { }
        public Trigger(IContainer container)
        {
            container?.Add(this);
            InitializeComponent();
        }
        protected virtual void OnFired(FiredEventArgs e) => Fired?.Invoke(this, e);
        public event EventHandler<FiredEventArgs> Fired;
    }
    public class FiredEventArgs : EventArgs
    {
    }
}
