using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Lib.Windows.Controls.Design
{
    public class ComponentEditor : UITypeEditor
    {
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context?.Instance == null) return value;
            if (context?.Container == null) return value;

            var service = provider?.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            if (service == null) return value;

            var comp = context.Container.Components.Cast<Component>().Where(_ => _.GetType() == context.PropertyDescriptor.PropertyType);

            using (var box = new ListBox())
            {
                box.SelectionMode = SelectionMode.One;
                service.DropDownControl(box);
                return box.SelectedItem ?? value;
            }
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) => UITypeEditorEditStyle.DropDown;
    }
}
