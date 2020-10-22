using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lib.Windows.Controls
{
    public class DragDropTrace : MouseTrace
    {
        readonly static MouseButtons _all;
        static DragDropTrace()
        {
            foreach (var i in Enum.GetValues(typeof(MouseButtons)).Cast<MouseButtons>()) _all |= i;
        }
        public DragDropTrace() : base() { }
        public DragDropTrace(IContainer container) : base(container) { }
        [DefaultValue(typeof(MouseButtons), "None")] public MouseButtons DragButton { get; set; }
        [Browsable(false)] public MouseButtons Draged { get; private set; }
        [Browsable(false)] public bool IsDragging => Draged > 0;
        [Browsable(false)] public virtual Point Location0 { get; protected set; }
        [Browsable(false)] public virtual Point Location1 { get; protected set; }
        public event EventHandler<DragDropEventArgs> Drag;
        public event EventHandler<DragDropEventArgs> Drop;
        public event EventHandler Canceled;
        EventHandler<MouseEventArgs> _move;
        protected virtual void OnDrag() => Drag?.Invoke(this, new DragDropEventArgs(Draged, Location0, Location1));
        protected virtual void OnDrop() => Drop?.Invoke(this, new DragDropEventArgs(Draged, Location0, Location1));
        protected override void OnDown(object sender, MouseEventArgs e)
        {
            if (IsDragging)
            {
                base.OnDown(sender, e);
                if (Pushing.HasFlag(MouseButtons.Left | MouseButtons.Right)) Cancel();
            }
            else
            {
                Draged |= e.Button;
                base.OnDown(sender, e);
                if (DragButton == MouseButtons.None ||
                    DragButton.HasFlag(e.Button))
                {
                    Location0 = e.Location;
                    Location1 = e.Location;
                    OnDrag();
                    _move = OnMoveOnDrag;
                }
            }
        }
        protected override void OnMove(object sender, MouseEventArgs e)
        {
            base.OnMove(sender, e);
            _move?.Invoke(sender, e);
        }
        protected virtual void OnMoveOnDrag(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.None && Draged.HasFlag(e.Button))
            {
                if (DragButton == MouseButtons.None ||
                     DragButton.HasFlag(e.Button))
                {
                    Location1 = e.Location;
                    OnDrag();
                }
            }
        }
        protected override void OnUp(object sender, MouseEventArgs e)
        {
            if (Draged.HasFlag(e.Button))
            {
                if (DragButton == MouseButtons.None ||
                    DragButton.HasFlag(e.Button))
                {
                    Location1 = e.Location;
                    OnDrop();
                    _move = null;
                }
                Draged &= ~e.Button;
            }
            base.OnUp(sender, e);
        }
        protected virtual void OnCanceled() => Canceled?.Invoke(this, EventArgs.Empty);
        public void Cancel(MouseButtons button)
        {
            Draged &= ~button;
            OnCanceled();
            _move = null;
        }
        public void Cancel() => Cancel(_all);
    }
    public class DragDropEventArgs : EventArgs
    {
        public MouseButtons Button { get; }
        public Point Location0 { get; }
        public Point Location1 { get; }
        public Point Location => Location1;
        public DragDropEventArgs(MouseButtons button, Point location0, Point location1)
        {
            Button = button;
            Location0 = location0;
            Location1 = location1;
        }
        public Point Vector => Point.Subtract(Location1, new Size(Location0));
    }
}
 