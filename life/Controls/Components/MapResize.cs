using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib.Windows.Controls;

namespace life.Controls
{
	public partial class MapResize : Component
	{
		bool _auto = true;
		[DefaultValue(10)] public int Padding { get; set; } = 10;
		[DefaultValue(true)] public bool Auto 
		{
			get => _auto;
			set
			{
				if (_auto == value) return;
				_auto = value;
				if (Owner != null)
				{
					RemoveHandler(Owner);
					AddHandler(Owner);
					Owner.Map.Size = Owner.ClientSize;
				}
			}
		}
		[Browsable(false)] public Size Size { get => Owner?.Map.Size ?? Size.Empty; set { if (Owner != null) Owner.Map.Size = value; } }
		public MapResize() : this(null) { }
		public MapResize(IContainer container)
		{
			container?.Add(this);
			InitializeComponent();
		}
		protected override void AddHandler(MapView owner)
		{
			base.AddHandler(owner);
			if (Auto)
			{
				owner.Resize += OnResize;
			}
			else
			{
				owner.Layer.Add(_flame);
				owner.Mouse.Move += OnMove;
				owner.Mouse.Down += OnDown;
				owner.Mouse.Canceled += OnCanceled;
			}
		}
		protected override void RemoveHandler(MapView owner)
		{
			base.RemoveHandler(owner);
			if (owner.Layer.Contains(_flame)) owner.Layer.Remove(_flame);
			owner.Mouse.Move -= OnMove;
			owner.Mouse.Down -= OnDown;
			owner.Mouse.Drag -= OnDrag;
			owner.Mouse.Drop -= OnDrop;
			owner.Mouse.Canceled -= OnCanceled;
			owner.Resize -= OnResize;
		}
		public void Cancel() => Owner?.Mouse.Cancel();
		ResizeDirection _direction;
		void OnMove(object sender, MouseEventArgs e)
		{
			Owner.Cursor = GetCursor(e.Location);
		}
		void OnDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				_direction = GetDirection(e.Location);
				if (_direction != ResizeDirection.None)
				{
					Owner.Mouse.Drag += OnDrag;
					Owner.Mouse.Drop += OnDrop;
				}
			}
		}
		void OnDrag(object sender, DragDropEventArgs e)
		{
			Owner.Cursor = GetCursor();
			_flame.Rectangle = GetResizedRectangle(e.Vector);
			_flame.Show();
		}
		void OnDrop(object sender, DragDropEventArgs e)
		{
			var rect = GetResizedRectangle(e.Vector);
			Owner.Drawer.Location = rect.Location;
			Owner.Map.Size = rect.Size;
			_flame.Hide();
			Cancel();
		}
		void OnCanceled(object sender, EventArgs e)
		{
			_direction = ResizeDirection.None;
			Owner.Cursor = Cursors.Default;
			Owner.Mouse.Drag -= OnDrag;
			Owner.Mouse.Drop -= OnDrop;
		}
		void OnResize(object sender, EventArgs e)
		{
			Owner.Map.Size = Owner.ClientSize;
		}
		ResizeDirection GetDirection(Point location)
		{
			var dir = ResizeDirection.None;
			var @in = Owner?.MapRectangle ?? Rectangle.Empty;
			var @out = @in;
			@out.Inflate(Padding, Padding);
			if (new Rectangle(@out.Left, @out.Top, @out.Width, 10).Contains(location)) dir |= ResizeDirection.N;
			if (new Rectangle(@out.Left, @in.Bottom, @out.Width, 10).Contains(location)) dir |= ResizeDirection.S;
			if (new Rectangle(@out.Left, @out.Top, 10, @out.Height).Contains(location)) dir |= ResizeDirection.W;
			if (new Rectangle(@in.Right, @out.Top, 10, @out.Height).Contains(location)) dir |= ResizeDirection.E;
			return dir;
		}
		Cursor GetCursor(Point location) => GetCursor(GetDirection(location));
		Cursor GetCursor() => GetCursor(_direction);
		Cursor GetCursor(ResizeDirection direction)
		{
			switch (direction)
			{
				case ResizeDirection.None: return Cursors.Default;
				case ResizeDirection.N: case ResizeDirection.S: return Cursors.SizeNS;
				case ResizeDirection.W: case ResizeDirection.E: return Cursors.SizeWE;
				case ResizeDirection.NW: case ResizeDirection.SE: return Cursors.SizeNWSE;
				case ResizeDirection.NE: case ResizeDirection.SW: return Cursors.SizeNESW;
				default: return Cursors.Default;
			}
		}
		Rectangle GetResizedRectangle(Point vector)
		{
			var location = Owner.Drawer.Location;
			var scale = Owner.Drawer.Scale;
			var size = Owner.Map.Size;
			if (_direction.HasFlag(ResizeDirection.N))
			{
				if (size.Height - vector.Y / scale < 0)
				{
					location.Y += (size.Height - 1) * scale;
					size.Height = 1;
				}
				else
				{
					location.Y += vector.Y;
					size.Height -= vector.Y / scale;
				}
			}
			if (_direction.HasFlag(ResizeDirection.S))
			{
				size.Height += vector.Y / scale;
				if (size.Height < 0) size.Height = 1;
			}
			if (_direction.HasFlag(ResizeDirection.W))
			{
				if (size.Width - vector.X / scale < 0)
				{
					location.X += (size.Width - 1) * scale;
					size.Width = 1;
				}
				else
				{
					location.X += vector.X;
					size.Width -= vector.X / scale;
				}
			}
			if (_direction.HasFlag(ResizeDirection.E))
			{
				size.Width += vector.X / scale;
				if (size.Width < 0) size.Width = 1;
			}
			return new Rectangle(location, size);
		}
	}
	[Flags]
	public enum ResizeDirection
	{
		None = 0x00,
		N = 0x01,
		S = 0x02,
		W = 0x04,
		E = 0x08,
		NW = N | W,
		NE = N | E,
		SW = S | W,
		SE = S | E,
	}
}
