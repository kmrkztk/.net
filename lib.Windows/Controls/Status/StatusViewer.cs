using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;

namespace Lib.Windows.Controls.Status
{
    public partial class StatusViewer : UserControl
    {
        IStatus _obj;
        PropertyInfo _pi;
        string _format;
        string _name;
        readonly EventHandler _updated;
        public StatusViewer()
        {
            InitializeComponent();
            _updated = Updated;
        }
        public StatusViewer(IStatus obj, string propertyName) : this() => SetTarget(obj, propertyName);
        public StatusViewer(IStatus obj, PropertyInfo pi) : this() => SetTarget(obj, pi);
        public void SetTarget(IStatus obj, string propertyName) => SetTarget(obj, obj.GetType().GetProperty(propertyName));
        public void SetTarget(IStatus obj, PropertyInfo pi)
        {
            if (_obj != null) _obj.Updated -= _updated;
            _obj = obj;
            _pi = pi;
            var a = pi.GetCustomAttributes(typeof(StatusAttribute), false).Cast<StatusAttribute>().FirstOrDefault();
            _format = a?.Format ?? "{0} {1}";
            _name = a?.Name ?? pi.Name;
            _obj.Updated += _updated;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (_obj == null) return;
            using (var b = new SolidBrush(ForeColor)) e.Graphics.DrawString(
                string.Format(_format, _name, _pi.GetValue(_obj)),
                this.Font,
                b,
                Point.Empty);
        }
        void Updated(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(Refresh));
                return;
            }
            Refresh();
        }
    }
}
