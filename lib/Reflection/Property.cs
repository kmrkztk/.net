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
        public string Name => Info.Name;
        public bool IsValue => Info.HasAttribute<CommandValueAttribute>();
        public bool IsList => PropertyType.IsGenericType && PropertyType.GetGenericTypeDefinition() == typeof(List<>);
        public char? ValueSeparator => Info.GetCustomAttribute<CommandValueAttribute>()?.Separator;
        public Property(object instance, PropertyInfo pi)
        {
            Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            Info = pi ?? throw new ArgumentNullException(nameof(pi));
        }
        public object GetValue() => Info.GetValue(Instance);
        public void SetValue(object value) => Info.SetValue(Instance, PropertyType.Cast(value));
        public void AddValue(object value)
        {
            if (IsList)
            {
                var list = (GetValue() as IList) ?? Activator.CreateInstance(PropertyType) as IList;
                list.Add(PropertyType.GetGenericArguments()[0].Cast(value));
                SetValue(list);
            }
            else SetValue(value);
        }
        public override string ToString() => string.Format("{0}.{1} = {2}", Instance.GetType().Name, Info.Name, Info.GetValue(Instance));

        public static IEnumerable<Property> GetProperties(object instance) => instance.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Select(_ => new Property(instance, _));
        public static void Copy(object a, object b)
        {
            if (a == null || b == null) return;
            var ap = GetProperties(a);
            if (a.GetType() == b.GetType())
            {
                foreach (var p in ap) p.Info.SetValue(b, p.GetValue());
            }
            else
            {
                var bp = GetProperties(b);
                foreach (var p in ap)
                {
                    var p_ = bp.FirstOrDefault(_ => _.Name == p.Name);
                    if (p_ == null) continue;
                    p_.SetValue(p.GetValue());
                }
            }
        }
    }
}
