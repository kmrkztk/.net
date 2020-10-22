using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace life.Controls
{
	public partial class DrawScale : Component
	{
		[DefaultValue(10)] public int MaxScale { get; set; } = 10;
		public DrawScale() : this(null) { }
		public DrawScale(IContainer container)
		{
			container?.Add(this);
			InitializeComponent();
		}
        protected override void AddHandler(MapView owner)
        {
            base.AddHandler(owner);
			owner.Mouse.Wheel += OnWheel;
		}
		protected override void RemoveHandler(MapView owner)
		{
			base.RemoveHandler(owner);
			owner.Mouse.Wheel -= OnWheel;
		}
		void OnWheel(object sender, MouseEventArgs e) => Zoom(e.Delta / 120);
		public void Zoom(int delta) => Zoom(new Point((Owner?.ClientSize.Width ?? 0) / 2, (Owner?.ClientSize.Height ?? 0) / 2), delta);
		public void Zoom(Point location, int delta) => Zoom(Owner.Layer, location, delta);
		void Zoom(Lib.Windows.Gaming.Layer layer, Point location, int delta)
		{
			foreach (var d in layer.Cast<Lib.Windows.Gaming.Drawer>()) Zoom(d, location, delta);
		}
		void Zoom(Lib.Windows.Gaming.Drawer drawer, Point location, int delta)
		{
			if (drawer is MapDrawer m) Zoom(m, location, delta);
			Zoom(drawer.Layer, location, delta);
		}
		void Zoom(MapDrawer drawer, Point location, int delta)
		{
			var s = drawer.Scale + delta;
			if (s < 1) s = 1;
			if (MaxScale > 0 && s > MaxScale) s = MaxScale;
			var p = drawer.Location;
			p = Point.Subtract(p, new Size(location));
			p = new Point(p.X * s / drawer.Scale, p.Y * s / drawer.Scale);
			p = Point.Add(p, new Size(location));
			drawer.Area = new DrawArea(p, s);
		}
		public void ZoomIn() => Zoom(1);
		public void ZoomIn(Point location) => Zoom(location, 1);
		public void ZoomOut() => Zoom(-1);
		public void ZoomOut(Point location) => Zoom(location, -1);
	}
}
