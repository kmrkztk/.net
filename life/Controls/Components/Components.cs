using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Windows.Controls;
using Lib.Windows.Controls.Design;

namespace life.Controls
{
    [Editor(typeof(ComponentsEditor), typeof(UITypeEditor))]
    public class Components : ComponentCollection<Component>
    {
        readonly MapView _owner;
        public Components(MapView owner) => _owner = owner;
        protected override void InsertItem(int index, Component item)
        {
            item.Owner = _owner;
            base.InsertItem(index, item);
        }
    }
    public class ComponentsEditor : ComponentCollectionEditor<Component>
    {
        public ComponentsEditor(Type type) : base(type) { }
    }
}
