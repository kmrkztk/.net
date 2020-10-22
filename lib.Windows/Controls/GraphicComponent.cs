using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib.Windows.Drawing;

namespace Lib.Windows.Controls
{
    public partial class GraphicComponent : Component
    {
        DoubleBuffer _db;
        Control _c;
        public override IComponent Parent
        {
            get => base.Parent;
            set
            {
                base.Parent = value;
                _db?.Dispose();
                _db = null;
                if (value is Control c)
                {
                    _c = c;
                    _db = new DoubleBuffer(c);
                }
            }
        }
        public Graphics Graphics => _db?.Graphics;
        public GraphicComponent() : this(null) { }
        public GraphicComponent(IContainer container)
        {
            container?.Add(this);
            InitializeComponent();
            Disposed += (sender, e) => _db?.Dispose();
        }
        public void Render()
        {    
            if (_db == null) return;
            if (_c?.InvokeRequired ?? true) _c?.Invoke((Action)_db.Render); else _db?.Render();
        }
    }
}
