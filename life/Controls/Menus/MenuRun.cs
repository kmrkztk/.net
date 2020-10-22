using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public class MenuRun : MenuItem
    {
        public override string Text { get => (Component?.IsRunning ?? false) ? "Stop(&S)" : "Run(&S)"; set => base.Text = value; }
        public override bool Enabled { get => Component != null; set => base.Enabled = value; }
        public LifeEngine Component { get; set; }
        public MenuRun() : this(null) { }
        public MenuRun(IContainer container) : base(container) => this.ShortcutKeys = Keys.F5;
        public override void Do()
        {
            if (Component == null) return;
            if (Component.IsRunning) Component.Stop(); else Component.Run();
        }
    }
}
