using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls.Menus
{
    public class MenuFile : MenuItem
    {
        public override string Text { get => "File(&F)"; set => base.Text = value; }
        public override bool Enabled { get => Component != null; set => base.Enabled = value; }
        [DefaultValue(null)] public MapFile Component { get; set; }
        public MenuFile() : this(null) { }
        public MenuFile(IContainer container) : base(container) { }
    }
}
