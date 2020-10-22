using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public class MenuSize : MenuItem
    {
        public override string Text { get => "Size(&Z)"; set => base.Text = value; }
        public override bool Enabled { get => Component != null; set => base.Enabled = value; }
        [DefaultValue(null)] public MapResize Component { get; set; }
        public MenuSize() : this(null) { }
        public MenuSize(IContainer container) : base(container) { }
        public override void Do()
        {
            if (Component == null) return;
            using (var dialog = new SizeDialog(Component))
            {
                dialog.ShowDialog();
                Component.Auto = dialog.Auto;
                if (!dialog.Auto) Component.Size = dialog.MapSize;
            }
        }
    }
}
