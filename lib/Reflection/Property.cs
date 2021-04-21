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
        public Property(object instance, PropertyInfo pi)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Info = pi ?? throw new ArgumentNullException(nameof(pi));
        }
        public object GetValue() => Info.GetValue(Instance);
        public void SetValue(object value) => Info.SetValue(Instance, ConvertType(value, PropertyType));
        public void AddValue(object value)
        {
            if (IsList)
            {
                var list = (GetValue() as IList) ?? Activator.CreateInstance(PropertyType) as IList;
                list.Add(ConvertType(value, PropertyType.GetGenericArguments()[0]));
                SetValue(list);
            }
            else SetValue(value);
        }
        protected static object ConvertType(object value, Type type) => value.GetType() == type ? value : Convert.ChangeType(value, type);
        public override string ToString() => string.Format("{0}.{1} = {2}", Instance.GetType().Name, Info.Name, Info.GetValue(Instance));

        public static IEnumerable<Property> GetProperties(object instance) => instance.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Select(_ => new Property(instance, _));
    }
}
