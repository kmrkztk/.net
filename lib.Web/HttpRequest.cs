using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Lib.Web
{
    public delegate HttpRequestMessage RequestGenerator(Uri uri, HttpMethod method);
    public class HttpRequest
    {
        public HttpClient Client { get; }
        public Uri Uri { get; }
        public HttpMethod Method { get; }
        public RequestGenerator RequestGenerator { get; set; } = (uri, method) => new HttpRequestMessage(method, uri);
        public HttpRequest(HttpClient client, string uri) : this(client, uri, HttpMethod.Get) { }
        public HttpRequest(HttpClient client, string uri, HttpMethod method) : this(client, new Uri(uri), method) { }
        public HttpRequest(HttpClient client, Uri uri) : this(client, uri, HttpMethod.Get) { }
        public HttpRequest(HttpClient client, Uri uri, HttpMethod method)
        {
            Client = client;
            Uri = uri;
            Method = method;
        }
        public HttpRequest Of() => new(Client, Uri, Method) { RequestGenerator = RequestGenerator, };
        public HttpRequest Of(Uri uri) => new(Client, new Uri(Uri, uri), Method) { RequestGenerator = RequestGenerator, };
        public HttpRequest Of(string uri) => Of(new Uri(uri));
        public HttpRequest Of(QueryParameter parameter) => Of("?" + parameter.ToString());
        public HttpRequest Of(PathParameter parameter) => new(Client, parameter.Replace(Uri), Method) { RequestGenerator = RequestGenerator, };
        public HttpRequest OfPath(IList<object> parameter) => Of(new PathParameter(parameter));
        public HttpRequest OfPath(params object[] parameter) => OfPath(parameter);
        public HttpRequest Of(HttpMethod method) => new(Client, Uri, method) { RequestGenerator = RequestGenerator, };
        public HttpRequest OfGet()      => Of(HttpMethod.Get);
        public HttpRequest OfPost()     => Of(HttpMethod.Post);
        public HttpRequest OfPut()      => Of(HttpMethod.Put);
        public HttpRequest OfDelete()   => Of(HttpMethod.Delete);
        public HttpRequest OfTrace()    => Of(HttpMethod.Trace);
        public HttpRequest OfPatch()    => Of(HttpMethod.Patch);
        public T Of<T>() where T : HttpRequest => (T)Of(UriAttribute.GetUri<T>());
        public HttpResponse Call() => Call(CancellationToken.None);
        public HttpResponse Call(CancellationToken cancel) => Call(HttpCompletionOption.ResponseContentRead, cancel);
        public HttpResponse Call(HttpCompletionOption option) => Call(option, CancellationToken.None);
        public HttpResponse Call(HttpCompletionOption option, CancellationToken cancel) => Call(null, option, cancel);
        public HttpResponse Call(HttpContent content) => Call(content, CancellationToken.None);
        public HttpResponse Call(HttpContent content, CancellationToken cancel) => Call(content, HttpCompletionOption.ResponseContentRead, cancel);
        public HttpResponse Call(HttpContent content, HttpCompletionOption option) => Call(content, option, CancellationToken.None);
        public HttpResponse Call(HttpContent content, HttpCompletionOption option, CancellationToken cancel)
        {
            var request = RequestGenerator.Invoke(Uri, Method);
            request.Content = content;
            var response = Client.Send(request, option, cancel);
            return new(response);
        }
    }
}
