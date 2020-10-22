using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls
{
    public partial class MapData : System.ComponentModel.Component
    {
        Map _map = Maps.Empty;
        public event EventHandler Updated;
        public event EventHandler Resized;
        protected void OnUpdated() => Updated?.Invoke(this, EventArgs.Empty);
        protected void OnResize() => Resized?.Invoke(this, EventArgs.Empty);
        [Browsable(false)] public int Width => _map.Width;
        [Browsable(false)] public int Height => _map.Height;
        [Browsable(false)]
        public Size Size 
        {
            get => _map.Size; 
            set => Lock(() => Update(() => 
            {
                value = value.IsEmpty ? new Size(1, 1) : value;
                if (_map.Size == value) return;
                _map.Resize(value); 
                OnResize(); 
            })); 
        }
        [Browsable(false)] public Map Map { get => _map; set => Lock(() => Update(() => _map = value ?? Maps.Empty)); }
        public MapData() : this(null) { }
        public MapData(IContainer container)
        {
            container?.Add(this);
            InitializeComponent();
        }
        public void Lock(Action action) { lock (_map) action.Invoke(); }
        public T Lock<T>(Func<T> action) { lock (_map) return action.Invoke(); }
        public void Update(Action action) { action.Invoke(); OnUpdated(); }
        public void Update(Func<bool> action) { if (action.Invoke()) OnUpdated(); }
        public void Clear() => Lock(() => Update(_map.Clear));
        public void Superimpose(Point location, Map map) => Update(() => Lock(() => _map.Superimpose(location, map)));
        public void Overlap(Point location, Map map) => Update(() => Lock(() => _map.Overlap(location, map)));
        public void Rotate(int direction) => Update(() => Lock(() => _map = _map.Rotate(direction)));
        public void Reverse() => Update(() => Lock(() => _map = _map.Reverse()));
        public void UpsideDown() => Update(() => Lock(() => _map = _map.UpsideDown()));
        public void Next() => Lock(() => _map = Calculator.Next(_map));
    }
}
