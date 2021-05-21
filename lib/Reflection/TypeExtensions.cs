using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Reflection
{
    public static class TypeExtensions
    {
        public static dynamic Dynamic(object obj) => (dynamic)obj;
        public static T Cast<T>(this object obj) => (T)Cast(typeof(T), obj);
        public static dynamic Cast(this Type type, object obj) => 
            obj == null ? obj :
            obj.GetType() == type ? obj : 
            obj.GetType().IsAssignableTo(type) ? obj :
            TypeDescriptor.GetConverter(type).ConvertFrom(obj);
    }
}
