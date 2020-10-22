using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public class MenuSave : MenuFile
    {
        public override string Text { get => "Save(&S)"; set => base.Text = value; }
        public override bool Enabled { get => Component != null && Component.Savable; set => base.Enabled = value; }
        public override void Do() => Component?.Save();
        public MenuSave() : this(null) { }
        public MenuSave(IContainer container) : base(container) => this.ShortcutKeys = Keys.Control | Keys.S;
    }
}
