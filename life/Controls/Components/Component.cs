using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls
{
    public class Component : Lib.Windows.Controls.Component
    {
        public Component() : this(null) { }
        public Component(IContainer container)
        {
            container?.Add(this);
        }
        public override IComponent Parent
        {
            get => base.Parent;
            set
            {
                base.Parent = value;
                Owner = Owner ?? value as MapView;
            }
        }
        MapView _owner;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MapView Owner 
        {
            get => _owner; 
            set 
            {
                if (_owner != null) RemoveHandler(_owner);
                _owner = value; 
                if (_owner != null) AddHandler(_owner);
            }
        }
        protected virtual void AddHandler(MapView owner) { }
        protected virtual void RemoveHandler(MapView owner) { }
    }
}
