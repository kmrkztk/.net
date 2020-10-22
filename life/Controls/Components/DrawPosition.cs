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
	public partial class DrawPosition : Component
	{
		public DrawPosition() : this(null) { }
		public DrawPosition(IContainer container)
		{
			container?.Add(this);
			InitializeComponent();
		}
        protected override void AddHandler(MapView owner)
        {
			base.AddHandler(owner);
			owner.Resize += OnResize;
			owner.Drawer.DrawAreaChanged += OnZoomed;
			owner.Mouse.Drag += OnDrag;
			owner.Mouse.Drop += OnDrop;
		}
		protected override void RemoveHandler(MapView owner)
		{
			base.RemoveHandler(owner);
			owner.Resize -= OnResize;
			owner.Drawer.DrawAreaChanged -= OnZoomed;
			owner.Mouse.Drag -= OnDrag;
			owner.Mouse.Drop -= OnDrop;
		}
		Point _location;
		void OnDrag(object sender, DragDropEventArgs e)
		{
			if (e.Button != MouseButtons.Right) return;
			if (_location.IsEmpty)
			{
				_location = e.Location;
				return;
			}
			Offset(_location, e.Location);
			_location = e.Location;
		}
		void OnDrop(object sender, DragDropEventArgs e) => _location = Point.Empty;
		void OnZoomed(object sender, DrawAreaChangedEventArgs e) { if (e.IsZoomed) Adjust(); }
		void OnResize(object sender, EventArgs e) { Adjust(); }
		void OnUpdated(object sender, EventArgs e) => Adjust();
		public void Offset(Point p1, Point p2) => Offset(Point.Subtract(p2, new Size(p1)));
		public void Offset(Point p)
		{
			var offset = Owner.Drawer.Location;
			offset.Offset(p);
			Adjust(offset);
		}
		public void Adjust(Point p)
		{
			int adjust(int point, int msize, int dsize)
			{
				if (msize > dsize)
				{
					if (point > 0) return 0;
					if (point + msize < dsize) return dsize - msize;
				}
				else if (msize < dsize)
				{
					if (point < 0) return 0;
					if (point + msize > dsize) return dsize - msize;
				}
				else
				{
					return 0;
				}
				return point;
			}
			var rect = Owner.MapRectangle;
			p.X = adjust(p.X, rect.Width, Owner.ClientSize.Width);
			p.Y = adjust(p.Y, rect.Height, Owner.ClientSize.Height);
			if (Owner.Drawer.Location == p) return;
			Owner.Drawer.Location = p;
		}
		public void Adjust() => Adjust(Owner.Drawer.Location);
	}
}
