using System;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Lib.Jsons;

namespace Lib.Web
{
    public class HttpResponse
    {
        readonly HttpResponseMessage _message;
        public HttpResponseMessage Message => _message;
        public HttpStatusCode StatusCode => _message.StatusCode;
        public HttpResponseHeaders Headers => _message.Headers;
        public HttpResponseHeaders TrailingHeaders => _message.TrailingHeaders;
        public HttpContent Content => _message.Content;
        public HttpRequestMessage RequestMessage => _message.RequestMessage;
        public bool IsSuccessStatusCode => _message.IsSuccessStatusCode;
        public string ReasonPhrase => _message.ReasonPhrase;
        public Version Version => _message.Version;
        public HttpResponseBody Body => new(Content);
        public HttpResponse(HttpResponseMessage message) => _message = message;
        ~HttpResponse() => _message.Dispose();
    }
    public class HttpResponseBody
    {
        readonly HttpContent _content;
        public HttpResponseBody(HttpContent content) => _content = content;
        public HttpContentHeaders Headers => _content.Headers;
        public string AsString() => AsStringAsync().Result;
        public Stream AsStream() => _content.ReadAsStream();
        public Json AsJson() => Json.Load(AsStream());
        public XmlDocument AsXml()
        {
            var xml = new XmlDocument();
            xml.Load(AsStream());
            return xml;
        }
        public async Task<string> AsStringAsync() => await AsStringAsync(CancellationToken.None);
        public async Task<string> AsStringAsync(CancellationToken cancel) => await _content.ReadAsStringAsync(cancel);
        public async Task<Stream> AsStreamAsync() => await AsStreamAsync(CancellationToken.None);
        public async Task<Stream> AsStreamAsync(CancellationToken cancel) => await _content.ReadAsStreamAsync(cancel);
        public async Task<Json> AsJsonAsync() => await AsJsonAsync(CancellationToken.None);
        public async Task<Json> AsJsonAsync(CancellationToken cancel) => Json.Load(await AsStreamAsync(cancel));
        public async Task<XmlDocument> AsXmlAsync() => await AsXmlAsync(CancellationToken.None);
        public async Task<XmlDocument> AsXmlAsync(CancellationToken cancel)
        {
            var xml = new XmlDocument();
            xml.Load(await AsStreamAsync(cancel));
            return xml;
        }
    }
}
