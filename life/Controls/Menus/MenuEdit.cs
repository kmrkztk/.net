using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public abstract class MenuEdit : MenuItem
    {
        public override string Text { get => "Edit(&E)"; set => base.Text = value; }
        public override bool Enabled { get => Component != null; set => base.Enabled = value; }
        [DefaultValue(null)] public Edit Component { get; set; }
        [Browsable(false)] public abstract Tool Tool { get; }
        public MenuEdit() : this(null) { }
        public MenuEdit(IContainer container) : base(container) => this.CheckOnClick = true;
        public override void Do()
        {
            if (Component == null) return;
            Component.Tool = Tool;
            Reset();
        }
        public void Reset()
        {
            void reset(ToolStripMenuItem item)
            {
                foreach (var i in item.DropDownItems.Cast<ToolStripMenuItem>())
                {
                    if (i is MenuEdit e) e.Checked = Component?.Tool != null && Component?.Tool == e.Tool;
                    reset(i);
                }
            }
            ToolStripMenuItem owner = this.OwnerItem as ToolStripMenuItem;
            while ((owner?.OwnerItem as ToolStripMenuItem) != null) owner = owner.OwnerItem as ToolStripMenuItem;
            reset(owner);
        }
    }
}
