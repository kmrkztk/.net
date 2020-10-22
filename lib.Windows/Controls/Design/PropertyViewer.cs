using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lib.Windows.Controls.Design
{
    public partial class PropertyViewer : Form
    {
        public PropertyViewer(object target) : this()
        {
            Text = target?.ToString();
            Grid.SelectedObject = target;
        }
        public PropertyViewer() => InitializeComponent();
        public PropertyGrid Grid { get => _grid; }
    }
}
