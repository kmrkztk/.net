using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace Lib.Web
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UriAttribute : Attribute
    {
        public Uri Uri { get; }
        public HttpMethod Method { get; } = HttpMethod.Get;
        public UriAttribute(string uri) : this(new Uri(uri)) { }
        public UriAttribute(Uri uri) => Uri = uri;
        public UriAttribute(string uri, HttpMethod method) : this(new Uri(uri), method) { }
        public UriAttribute(Uri uri, HttpMethod method) :this(uri) => Method = method;
        public static Uri GetUri<T>() => GetUri(typeof(T));
        public static Uri GetUri(Type type) => type.GetCustomAttributes(typeof(UriAttribute), false).Cast<UriAttribute>().FirstOrDefault()?.Uri;
    }
}
