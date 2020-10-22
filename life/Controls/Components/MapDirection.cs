using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Windows.Controls;

namespace life.Controls
{
    public partial class MapDirection : Component
    {
		public MapDirection() : this(null) { }
		public MapDirection(IContainer container)
		{
			container?.Add(this);
			InitializeComponent();
		}
	}
}
