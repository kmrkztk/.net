using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Web
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UriAttribute : Attribute
    {
        public Uri Uri { get; }
        public UriAttribute(string uri) : this(new Uri(uri)) { }
        public UriAttribute(Uri uri) => Uri = uri;
        public static Uri GetUri<T>() => GetUri(typeof(T));
        public static Uri GetUri(Type type) => type.GetCustomAttributes(typeof(UriAttribute), false).Cast<UriAttribute>().FirstOrDefault()?.Uri;
    }
}
