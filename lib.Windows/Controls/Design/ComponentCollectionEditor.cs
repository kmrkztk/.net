using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib.Reflection;

namespace Lib.Windows.Controls.Design
{
    public class ComponentCollectionEditor<T> : CollectionEditor where T : System.ComponentModel.Component
    {
        Type[] _types;
        public ComponentCollectionEditor(Type type) : base(type) { }
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            var d = provider?.GetService(typeof(ITypeDiscoveryService)) as ITypeDiscoveryService;
            _types = ReflectiveEnumerator.GetEnumerableOfType<T>(d.GetTypes(typeof(T), false).Cast<Type>().ToArray()).ToArray();
            return base.EditValue(context, provider, value);
        }
        protected override Type[] CreateNewItemTypes() => _types;
    }
}
