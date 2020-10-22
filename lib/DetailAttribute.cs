using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    public class DetailAttribute : Attribute
    {
        public string Content { get; set; }
        public DetailAttribute(string content) => Content = content;
        public static IEnumerable<string> GetContents(object obj)      => obj is PropertyInfo pi ? GetContents(pi) : obj.GetType().GetCustomAttributes<DetailAttribute>().Select(_ => _.Content);
        public static IEnumerable<string> GetContents(PropertyInfo pi) => pi.GetCustomAttributes<DetailAttribute>().Select(_ => _.Content);
    }
}
