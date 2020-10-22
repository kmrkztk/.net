using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls
{
    public partial class PatternListDialog : Form
    {
        public PatternList List => _list;
        public PatternListDialog()
        {
            InitializeComponent();
            _filter.TextChanged += (sender, e) => _list.Filtering = _filter.Text;
        }
    }
}
