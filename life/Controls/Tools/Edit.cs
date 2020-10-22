using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib.Windows.Controls;

namespace life.Controls
{
    public partial class Edit : Component
    {
        public Edit() : this(null) { }
        public Edit(IContainer container)
        {
            container?.Add(this);
            InitializeComponent();
        }
        protected override void AddHandler(MapView owner)
        {
            base.AddHandler(owner);
            owner.Mouse.Down += OnDown;
            owner.Mouse.Move += OnMove;
            owner.Mouse.Up += OnUp;
            owner.Mouse.Drag += OnDrag;
            owner.Mouse.Drop += OnDrop;
            owner.Mouse.Canceled += OnCanceled;
        }
        protected override void RemoveHandler(MapView owner)
        {
            base.RemoveHandler(owner);
            owner.Mouse.Down -= OnDown;
            owner.Mouse.Move -= OnMove;
            owner.Mouse.Up -= OnUp;
            owner.Mouse.Drag -= OnDrag;
            owner.Mouse.Drop -= OnDrop;
            owner.Mouse.Canceled -= OnCanceled;
        }
        ITool _tool;
        MapDrawer _shadow;
        [DefaultValue(null)] public ITool Tool 
        {
            get => _tool;
            set
            {
                if (_tool != null)
                {
                    _tool.Editing -= OnEditing;
                    _tool.Edited -= OnEdited;
                }
                _tool = value;
                if (_tool != null)
                {
                    _tool.Edit = this;
                    _tool.Editing += OnEditing;
                    _tool.Edited += OnEdited;
                }
                if (_shadow != null) _shadow.Map = _tool?.Map;
            }
        }
        [DefaultValue(null)] public virtual MapDrawer Shadow 
        {
            get => _shadow;
            set
            {
                _shadow = value;
                if (_shadow != null) _shadow.Map = _tool?.Map;
            }
        }
        void OnDown(object sender, MouseEventArgs e) { Tool?.Down(e); }
        void OnMove(object sender, MouseEventArgs e) { Tool?.Move(e); }
        void OnUp(object sender, MouseEventArgs e) { Tool?.Up(e); }
        void OnDrag(object sender, DragDropEventArgs e) { Tool?.Drag(e); }
        void OnDrop(object sender, DragDropEventArgs e) { Tool?.Drop(e); }
        void OnCanceled(object sender, EventArgs e) { Tool?.Cancel(); }
        void OnEditing(object sender, EventArgs e)
        {
            if (Shadow == null) return;
            Shadow.Location = Tool.Location;
            Shadow.Show();
        }
        void OnEdited(object sender, EventArgs e) => Shadow?.Hide();
        public void WriteSuperimpose() => Write(Owner.Superimpose);
        public void WriteOverlap() => Write(Owner.Overlap);
        void Write(Action<Point, Map> edit)
        {
            edit.Invoke(Tool.Location, Tool.Map.Map);
            Shadow?.Hide();
        }

    }
    public interface ITool
    {
        Point Location { get; }
        MapData Map { get; }
        Edit Edit { get; set; }
        void Down(MouseEventArgs e);
        void Move(MouseEventArgs e);
        void Up(MouseEventArgs e);
        void Drag(DragDropEventArgs e);
        void Drop(DragDropEventArgs e);
        void Cancel();
        event EventHandler Editing;
        event EventHandler Edited;
    }
}
