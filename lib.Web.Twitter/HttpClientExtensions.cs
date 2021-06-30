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
        public static Json ToJson(this HttpContent content) => Json.Load(content.ReadAsStream());
        public static async Task DownloadAsync(this HttpContent content, string filename) => await DownloadAsync(content, filename, CancellationToken.None);
        public static async Task DownloadAsync(this HttpContent content, string filename, FileMode mode) => await DownloadAsync(content, filename, mode, CancellationToken.None);
        public static async Task DownloadAsync(this HttpContent content, Stream stream) => await DownloadAsync(content, stream, CancellationToken.None);
        public static async Task DownloadAsync(this HttpContent content, string filename, CancellationToken cancellationToken) => await DownloadAsync(content, filename, FileMode.Create, cancellationToken);
        public static async Task DownloadAsync(this HttpContent content, string filename, FileMode mode, CancellationToken cancellationToken)
        {
            using var fs = new FileStream(filename, mode);
            await DownloadAsync(content, fs, cancellationToken);
        }
        public static async Task DownloadAsync(this HttpContent content, Stream stream, CancellationToken cancellationToken)
        {
            await content.ReadAsStream(cancellationToken).CopyToAsync(stream, cancellationToken);
        }
    }
}
