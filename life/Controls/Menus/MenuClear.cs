using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public class MenuClear : MenuItem
    {
        public override string Text { get => "Clear(&C)"; set => base.Text = value; }
        public override bool Enabled { get => Component != null; set => base.Enabled = value; }
        [DefaultValue(null)] public MapView Component { get; set; }
        public MenuClear() : this(null) { }
        public MenuClear(IContainer container) : base(container) => this.ShortcutKeys = Keys.Shift | Keys.Delete;
        public override void Do() => Component?.Map.Clear();
    }
}
