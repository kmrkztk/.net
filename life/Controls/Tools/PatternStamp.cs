using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Runtime.Remoting.Channels;

namespace life.Controls
{
    public partial class PatternStamp : Stamp
    {
        PatternListDialog _list;
        MapPreview _preview;
        public PatternStamp() : this(null) { }
        public PatternStamp(IContainer container) : base(container)
        {
        }

        public void Show()
        {
            if (_list?.IsDisposed ?? true)
            {
                _list = new PatternListDialog();
                _list.List.DoubleClick += (sender, e) => Preview();
                _list.List.SelectedMapChanged += (sender, e) => Preview(false);
                _list.List.SelectedMapChanged += (sender, e) => this.Map.Map = _list.List.SelectedMap?.Load();
                _list.Activated += (sender, e) => Activate(_preview);
                _list.FormClosing += (sender, e) => _preview?.Close();
            }
            _list.Show();
            _list.Activate();
        }
        public void Preview(bool show = true)
        {
            if (show && (_preview?.IsDisposed ?? true))
            {
                _preview = new MapPreview();
                _preview.Updated += (sender, e) => this.Map.Map = _preview.Map;
                _preview.Show();
            }
            _preview?.Set(_list.List.SelectedMap);
        }
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, uint uFlags);
        static void Activate(Form form)
        {
            if (form?.IsDisposed ?? true) return;
            const int HWND_TOPMOST = -1;
            //const int HWND_BOTTOM = 1;
            const int HWND_NOTOPMOST = -2;
            const uint SWP_NOSIZE = 0x0001;
            const uint SWP_NOMOVE = 0x0002;
            const uint SWP_NOACTIVATE = 0x0010;
            //const uint SWP_SHOWWINDOW = 0x0040;
            SetWindowPos(form.Handle, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
            SetWindowPos(form.Handle, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE);
        }
    }
}
