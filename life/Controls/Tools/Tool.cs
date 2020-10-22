using Lib.Windows.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls
{
    public partial class Tool : System.ComponentModel.Component, ITool
    {
        public Tool() : this(null) { }
        public Tool(IContainer container)
        {
            container?.Add(this);
            InitializeComponent();
        }
        [Browsable(false)] public Point Location { get; protected set; }
        [Browsable(false)] public MapData Map => _map;
        [Browsable(false)] public Edit Edit { get; set; }
        public event EventHandler Editing;
        public event EventHandler Edited;
        protected virtual void OnEditing() => Editing?.Invoke(this, EventArgs.Empty);
        protected virtual void OnEdited() => Edited?.Invoke(this, EventArgs.Empty);
        public virtual void Down(MouseEventArgs e) { }
        public virtual void Move(MouseEventArgs e) { }
        public virtual void Up(MouseEventArgs e) { }
        public virtual void Drag(DragDropEventArgs e) { }
        public virtual void Drop(DragDropEventArgs e) { }
        public virtual void Cancel() { }
    }
}
