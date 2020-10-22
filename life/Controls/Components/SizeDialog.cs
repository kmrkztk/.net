using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls
{
    public partial class SizeDialog : Form
    {
        public SizeDialog(MapResize resize) : this()
        {
            Auto = resize.Auto;
            MapSize = resize.Size;
        }
        public SizeDialog()
        {
            InitializeComponent();
            _auto.CheckedChanged += (sender, e) => panel1.Enabled = !_auto.Checked;
        }
        public bool Auto { get => _auto.Checked; set => _auto.Checked = value; }
        public Size MapSize
        {
            get => new Size((int)_width.Value, (int)_height.Value); 
            set
            {
                _width.Value = value.Width;
                _height.Value = value.Height;
            }
        }
    }
}
