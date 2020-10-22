using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public class MenuPatterns : MenuEdit
    {
        public override string Text { get => "Patterns..."; set => base.Text = value; }
        readonly PatternStamp _tool = new PatternStamp();
        public override Tool Tool => _tool;
        public MenuPatterns() : this(null) { }
        public MenuPatterns(IContainer container) : base(container)
        {
            this.ShortcutKeys = Keys.F6;
            _tool.Map.Updated += (sender, e) => base.Do();
        }
        public override void Do()
        {
            _tool.Show();
            Reset();
        }
    }
}
