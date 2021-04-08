using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Lib.Reflection
{
    public class Property
    {
        public object Instance { get; private set; }
        public PropertyInfo Info { get; private set; }
        public Type PropertyType => Info.PropertyType;
        public bool IsList => PropertyType.IsGenericType && PropertyType.GetGenericTypeDefinition() == typeof(List<>);
        public Property(object target, PropertyInfo pi)
        {
            Instance = target ?? throw new ArgumentNullException("target");
            Info = pi ?? throw new ArgumentNullException("pi");
        }
        public T GetValue<T>() => (T)Info.GetValue(Instance);
        public void SetValue(object value) => Info.SetValue(Instance, ConvertType(value, PropertyType));
        public void AddValue(object value)
        {
            if (IsList)
            {
                var list = GetValue<IList>() ?? Activator.CreateInstance(PropertyType) as IList;
                list.Add(ConvertType(value, PropertyType.GetGenericArguments()[0]));
                SetValue(list);
            }
            else SetValue(value);
        }
        protected object ConvertType(object value, Type type) => value.GetType() == type ? value : Convert.ChangeType(value, type);
        public override string ToString() => string.Format("{0}.{1} = {2}", Instance.GetType().Name, Info.Name, Info.GetValue(Instance));
    }
}
