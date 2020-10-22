using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Design;

namespace Lib.Windows.Controls
{
    public partial class MouseTrace : Trace
    {
        public MouseTrace() : this(null) { }
        public MouseTrace(IContainer container)
        {
            container?.Add(this);
            InitializeComponent();
        }
        [Browsable(false)] public virtual Point Location { get; protected set; }
        [Browsable(false)] public MouseButtons Pushing { get; private set; }
        [DefaultValue(typeof(MouseButtons), "None")] public MouseButtons Button { get; set; }
        public event EventHandler<MouseEventArgs> Down;
        public event EventHandler<MouseEventArgs> Move;
        public event EventHandler<MouseEventArgs> Up;
        public event EventHandler<MouseEventArgs> Wheel;
        protected virtual void OnDown(object sender, MouseEventArgs e)
        {
            Pushing |= e.Button;
            Location = e.Location;
            if (Button == MouseButtons.None || Button.HasFlag(e.Button)) Down?.Invoke(this, e);
        }
        protected virtual void OnMove(object sender, MouseEventArgs e)
        {
            Location = e.Location;
            if (e.Button == MouseButtons.None) Move?.Invoke(this, e);
        }
        protected virtual void OnUp(object sender, MouseEventArgs e)
        {
            Location = e.Location;
            Pushing &= ~e.Button;
            if (Button == MouseButtons.None || Button.HasFlag(e.Button)) Up?.Invoke(this, e);
        }
        protected virtual void OnWheel(object sender, MouseEventArgs e)
        {
            Location = e.Location;
            Wheel?.Invoke(this, e);
        }
        protected override void AddHandler(Control control)
        {
            control.MouseDown += OnDown;
            control.MouseUp += OnUp;
            control.MouseMove += OnMove;
            control.MouseWheel += OnWheel;
        }
        protected override void RemoveHandler(Control control)
        {
            control.MouseDown -= OnDown;
            control.MouseUp -= OnUp;
            control.MouseMove -= OnMove;
            control.MouseWheel -= OnWheel;
        }
    }
}
