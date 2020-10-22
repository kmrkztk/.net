using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib.Windows.Controls;

namespace Lib.Windows.Gaming
{
    public abstract partial class Drawer : Lib.Windows.Controls.Component
    {
        public override IComponent Parent 
        {
            get => base.Parent;
            set
            {
                base.Parent = value;
                Display = value as Display;
            }
        }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual Display Display { get; set; }
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Drawer Owner { get; set; }
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Layer Layer { get; private set; }
        [DefaultValue(true)] public bool Visible { get; set; } = true;
        public Drawer() : this(null) { }
        public Drawer(IContainer container)
        {
            container?.Add(this);
            InitializeComponent();
            Layer = new Layer(this);
        }
        public void Render()
        {
            if (Owner != null)
            {
                Owner.Render();
                return;
            }
            if (Display?.IsDisposed ?? true) return;
            Display?.Invoke(new Action(() =>
            {
                var g = Display.Graphics;
                Render(g);
                foreach (var d in Layer) d.Render(g);
                Display.Render();
            }));
        }
        public void Render(Graphics g)
        {
            if (Visible) Draw(g);
        }
        protected abstract void Draw(Graphics g);
        public void Show() { Visible = true; Render(); }
        public void Hide() { Visible = false; Render(); }
    }
}
