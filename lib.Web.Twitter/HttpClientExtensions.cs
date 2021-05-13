using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Lib.Jsons;

namespace Lib.Web.Twitter
{
    public static class HttpClientExtensions
    {
        public static Json GetJson(this HttpClient client, string requestUri) => GetJson(client, new Uri(requestUri));
        public static Json GetJson(this HttpClient client, string requestUri, CancellationToken cancellationToken) => GetJson(client, new Uri(requestUri), cancellationToken);
        public static Json GetJson(this HttpClient client, string requestUri, HttpCompletionOption completionOption) => GetJson(client, new Uri(requestUri), completionOption);
        public static Json GetJson(this HttpClient client, string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken) => GetJson(client, new Uri(requestUri), completionOption, cancellationToken);
        public static Json GetJson(this HttpClient client, Uri requestUri) => GetJson(client, requestUri, CancellationToken.None);
        public static Json GetJson(this HttpClient client, Uri requestUri, CancellationToken cancellationToken) => GetJson(client, requestUri, HttpCompletionOption.ResponseContentRead, cancellationToken);
        public static Json GetJson(this HttpClient client, Uri requestUri, HttpCompletionOption completionOption) => GetJson(client, requestUri, completionOption, CancellationToken.None);
        public static Json GetJson(this HttpClient client, Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
            => Json.Load(client.GetAsync(requestUri, completionOption, cancellationToken).Result.Content.ReadAsStream(cancellationToken));
        public static async Task<Json> GetJsonAsync(this HttpClient client, string requestUri) => await GetJsonAsync(client, new Uri(requestUri));
        public static async Task<Json> GetJsonAsync(this HttpClient client, string requestUri, CancellationToken cancellationToken) => await GetJsonAsync(client, new Uri(requestUri), cancellationToken);
        public static async Task<Json> GetJsonAsync(this HttpClient client, string requestUri, HttpCompletionOption completionOption) => await GetJsonAsync(client, new Uri(requestUri), completionOption);
        public static async Task<Json> GetJsonAsync(this HttpClient client, string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken) => await GetJsonAsync(client, new Uri(requestUri), completionOption, cancellationToken);
        public static async Task<Json> GetJsonAsync(this HttpClient client, Uri requestUri) => await GetJsonAsync(client, requestUri, CancellationToken.None);
        public static async Task<Json> GetJsonAsync(this HttpClient client, Uri requestUri, CancellationToken cancellationToken) => await GetJsonAsync(client, requestUri, HttpCompletionOption.ResponseContentRead, cancellationToken);
        public static async Task<Json> GetJsonAsync(this HttpClient client, Uri requestUri, HttpCompletionOption completionOption) => await GetJsonAsync(client, requestUri, completionOption, CancellationToken.None);
        public static async Task<Json> GetJsonAsync(this HttpClient client, Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken)
            => Json.Load(await (await client.GetAsync(requestUri, completionOption, cancellationToken)).Content.ReadAsStreamAsync(cancellationToken));
        public static async Task<Stream> DownloadAsync(this HttpClient client, string uri) => await DownloadAsync(client, uri, CancellationToken.None);
        public static async Task<Stream> DownloadAsync(this HttpClient client, string uri, CancellationToken cancellationToken) => await DownloadAsync(client, new Uri(uri), cancellationToken);
        public static async Task<Stream> DownloadAsync(this HttpClient client, Uri uri) => await DownloadAsync(client, uri, CancellationToken.None);
        public static async Task<Stream> DownloadAsync(this HttpClient client, Uri uri, CancellationToken cancellationToken)
        {
            var response = await client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            if (response.StatusCode != HttpStatusCode.OK) throw new HttpRequestException("", null, response.StatusCode);
            return await response.Content.ReadAsStreamAsync(cancellationToken);
        }
        public static async Task DownloadAsync(this HttpClient client, string uri, string filename) => await DownloadAsync(client, uri, filename, FileMode.Create, CancellationToken.None);
        public static async Task DownloadAsync(this HttpClient client, string uri, string filename, FileMode mode) => await DownloadAsync(client, uri, filename, mode, CancellationToken.None);
        public static async Task DownloadAsync(this HttpClient client, string uri, Stream stream) => await DownloadAsync(client, uri, stream, CancellationToken.None);
        public static async Task DownloadAsync(this HttpClient client, Uri uri, string filename) => await DownloadAsync(client, uri, filename, FileMode.Create, CancellationToken.None);
        public static async Task DownloadAsync(this HttpClient client, Uri uri, string filename, FileMode mode) => await DownloadAsync(client, uri, filename, mode, CancellationToken.None);
        public static async Task DownloadAsync(this HttpClient client, Uri uri, Stream stream) => await DownloadAsync(client, uri, stream, CancellationToken.None);
        public static async Task DownloadAsync(this HttpClient client, string uri, string filename, CancellationToken cancellationToken) => await DownloadAsync(client, new Uri(uri), filename, FileMode.Create, cancellationToken);
        public static async Task DownloadAsync(this HttpClient client, string uri, string filename, FileMode mode, CancellationToken cancellationToken) => await DownloadAsync(client, new Uri(uri), filename, mode, cancellationToken);
        public static async Task DownloadAsync(this HttpClient client, string uri, Stream stream, CancellationToken cancellationToken) => await DownloadAsync(client, new Uri(uri), stream, cancellationToken);
        public static async Task DownloadAsync(this HttpClient client, Uri uri, string filename, CancellationToken cancellationToken) => await DownloadAsync(client, uri, filename, FileMode.Create, cancellationToken);
        public static async Task DownloadAsync(this HttpClient client, Uri uri, string filename, FileMode mode, CancellationToken cancellationToken)
        {
            using var fs = new FileStream(filename, mode);
            await DownloadAsync(client, uri, fs, cancellationToken);
        }
        public static async Task DownloadAsync(this HttpClient client, Uri uri, Stream stream, CancellationToken cancellationToken)
        {
            using var sr = await DownloadAsync(client, uri, cancellationToken);
            sr.CopyTo(stream);
        }
    }
}
