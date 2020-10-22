using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace life.Controls
{
    public partial class Shadow : MapDrawerFull
    {
        public Shadow() : this(null) { }
        public Shadow(IContainer container)  
        {
            container?.Add(this);
            InitializeComponent();
        }
        byte _opacity;
        [DefaultValue(128)] public byte Opacity 
        {
            get => _opacity;
            set
            {
                _opacity = value;
                base.AliveColor = Color.FromArgb(_opacity, base.AliveColor);
                base.DeathColor = Color.FromArgb(_opacity, base.DeathColor);
            }
        }
        [DefaultValue(typeof(Color), "Lime")]  public new Color AliveColor { get => base.AliveColor; set => base.AliveColor = Color.FromArgb(_opacity, value); }
        [DefaultValue(typeof(Color), "Black")] public new Color DeathColor { get => base.DeathColor; set => base.DeathColor = Color.FromArgb(_opacity, value); }
    }
}
