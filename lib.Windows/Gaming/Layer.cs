using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Windows.Controls;
using Lib.Windows.Controls.Design;

namespace Lib.Windows.Gaming
{
    [Editor(typeof(LayerEditor), typeof(UITypeEditor))]
    public class Layer : ComponentCollection<Drawer>
    {
        readonly Drawer _owner;
        public Layer(Drawer owner) => _owner = owner;
        protected override void InsertItem(int index, Drawer item)
        {
            item.Owner = _owner;
            base.InsertItem(index, item);
        }
    }
    public class LayerEditor : ComponentCollectionEditor<Drawer>
    {
        public LayerEditor(Type type) : base(type) { }
    }
}
