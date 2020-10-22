using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lib.Windows.Drawing
{
    public class DoubleBuffer
    {
        BufferedGraphics _buf;
        readonly Control _control;
        readonly PaintEventHandler _paint;
        readonly EventHandler _resize;
        public Graphics Graphics => _buf?.Graphics;
        public DoubleBuffer(Control control)
        {
            _control = control ?? throw new ArgumentNullException();
            _buf = BufferedGraphicsManager.Current.Allocate(_control.CreateGraphics(), _control.DisplayRectangle);
            _paint = (sender, e) => Render();
            _resize = (sender, e) => Refresh();
            _control.Paint += _paint;
            _control.Resize += _resize;
            _control.Disposed += (sender, e) => Dispose();
        }
        public void Dispose()
        {
            _control.Paint -= _paint;
            _control.Resize -= _resize;
            _buf?.Dispose();
            _buf = null;
        }
        public void Render()
        {
            if (_buf == null) return;
            _buf.Render();
        }
        public void Refresh()
        {
            _buf?.Dispose();
            _buf = BufferedGraphicsManager.Current.Allocate(_control.CreateGraphics(), _control.DisplayRectangle);
        }
    }
}
