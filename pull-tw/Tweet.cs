using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;

namespace pull_tw
{
    class User
    {
        public string Name { get; init; }
        public string ID { get; init; }
        public override string ToString() => string.Format("({0}){1}", ID, Name);
    }
    class Media
    {
        public string ID { get; init; }
        public string Type { get; init; }
        public string Url { get; init; }
        public override string ToString() => Url;
        public async Task DownloadAsync()
        {
            using var handle = new HttpClientHandler()
            {
                Proxy = new WebProxy("proxy.fdc-inc.co.jp", 8080),
                UseProxy = true,
            };
            using var http = new HttpClient(handle);
            using var response = await http.GetAsync(Url, HttpCompletionOption.ResponseHeadersRead);
            if (response.StatusCode != HttpStatusCode.OK) throw new Exception();
            using var c = response.Content;
            using var sr = await c.ReadAsStreamAsync();
            using var fs = new FileStream(@".\" + ID, FileMode.CreateNew);
            sr.CopyTo(fs);
        }
    }
    class Tweet
    {
        public User User { get; init; }
        public string ID { get; init; }
        public string Text { get; init; }
        public IEnumerable<Media> Medias { get; init; }
        public override string ToString() => string.Format("[{0}]({1}){2}", User, ID, Text.Replace("\n", " ")) 
            + (Medias != null ? ("\r\n\t(medias)" + string.Join(",", Medias)) : "");
    }
}
