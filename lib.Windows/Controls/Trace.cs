using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lib.Windows.Controls
{
    public abstract partial class Trace : Component
    {
        public Trace() : this(null) { }
        public Trace(IContainer container)
        {
            container?.Add(this);
            InitializeComponent();
        }
        bool _enable = true;
        [Browsable(false)]
        public override IComponent Parent
        {
            get => base.Parent;
            set
            {
                if (Parent is Control a) RemoveHandler(a);
                base.Parent = value;
                if (_enable && Parent is Control b) AddHandler(b);
            }
        }
        [DefaultValue(true)]
        public bool Enabled
        {
            get => _enable;
            set
            {
                if (_enable == value) return;
                _enable = value;
                if (Parent is Control c) if (_enable) AddHandler(c); else RemoveHandler(c);
            }
        }
        protected abstract void AddHandler(Control control);
        protected abstract void RemoveHandler(Control control);
    }
}
