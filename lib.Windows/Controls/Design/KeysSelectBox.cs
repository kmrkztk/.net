using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lib.Windows.Controls.Design
{
    partial class KeysSelectBox : UserControl
    {
        public Keys Key
        {
            get => KeyCode | Modifiers;
            set
            {
                KeyCode = value & Keys.KeyCode;
                Modifiers = value & Keys.Modifiers;
            }
        }
        public Keys KeyCode
        {
            get => (Keys)_codes.SelectedItem;
            set => _codes.SelectedIndex = _codes.Items.IndexOf(value & Keys.KeyCode);
        }
        public Keys Modifiers
        {
            get => (_ctrl.Checked ? Keys.Control : Keys.None) |
                (_alt.Checked ? Keys.Alt : Keys.None) |
                (_shift.Checked ? Keys.Shift : Keys.None);
            set
            {
                _ctrl.Checked = value.HasFlag(Keys.Control);
                _alt.Checked = value.HasFlag(Keys.Alt);
                _shift.Checked = value.HasFlag(Keys.Shift);
            }
        }
        public KeysSelectBox()
        {
            InitializeComponent();
            _codes.Items.AddRange(Enum.GetValues(typeof(Keys)).Cast<Keys>().Where(_ => _ > Keys.Modifiers && _ < Keys.KeyCode).Cast<object>().ToArray());
            _codes.SelectedIndex = 0;
        }
    }
}
