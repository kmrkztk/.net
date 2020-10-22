using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Lib.Windows.Controls.Design
{
    public class KeyEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var service = provider?.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (service == null) return value;
            using (var box = new KeysSelectBox())
            {
                box.Key = (Keys)(value ?? Keys.None);
                service.DropDownControl(box);
                return box.Key;
            }
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.DropDown;
    }
}
