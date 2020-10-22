using Lib.Windows.Controls.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lib.Windows.Controls
{
    public partial class KeyTrace : Trace
    {
        public KeyTrace() : this(null) { }
        public KeyTrace(IContainer container)
        {
            container?.Add(this);
            InitializeComponent();
        }
        [Editor(typeof(KeyEditor), typeof(UITypeEditor))]
        [DefaultValue(typeof(Keys), "None")]
        public Keys Key { get; set; }
        [Browsable(false)] public bool Pushing { get; set; }
        public event EventHandler<KeyEventArgs> Down;
        public event EventHandler<KeyEventArgs> Up;
        protected virtual void OnDown(object sender, KeyEventArgs e)
        {
            if (Pushing) return;
            if ((Key & Keys.Modifiers) != e.Modifiers ||
                (Key & Keys.KeyCode)   != e.KeyCode) return;
            Pushing = true;
            Down?.Invoke(this, e);
        }
        protected virtual void OnUp(object sender, KeyEventArgs e)
        {
            if (!Pushing) return;
            if ((Key & Keys.Modifiers) != e.Modifiers && 
                (Key & Keys.KeyCode)   != e.KeyCode) return;
            Up?.Invoke(this, e);
            Pushing = false;
        }
        protected override void AddHandler(Control control)
        {
            control.KeyDown += OnDown;
            control.KeyUp += OnUp;
        }
        protected override void RemoveHandler(Control control)
        {
            control.KeyDown -= OnDown;
            control.KeyUp -= OnUp;
        }
    }
}
