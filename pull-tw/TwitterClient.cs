using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Lib;
using Lib.Json;

namespace pull_tw
{
    abstract partial class TwitterClient : HttpClient
    {
        protected readonly static HttpClientHandler _handle = new()
        {
            Proxy = new WebProxy("proxy.fdc-inc.co.jp", 8080),
            UseProxy = true,
        };
        protected readonly Uri _base;
        protected TwitterClient(string bearer, string ver) : base(_handle, true)
        {
            DefaultRequestHeaders.Authorization = new("Bearer", bearer);
            _base = new("https://api.twitter.com/" + ver);
        }
        protected Uri GetUri(string path) => new(_base.ToString() + path);
        protected static T Execute<T>(Task<T> task)
        {
            task.Wait();
            return task.Result;
        }
        public HttpResponseMessage Get(string uri) => Get(uri, CancellationToken.None);
        public HttpResponseMessage Get(string uri, CancellationToken cancel) => Get(new Uri(uri), cancel);
        public HttpResponseMessage Get(Uri uri) => Get(uri, CancellationToken.None);
        public HttpResponseMessage Get(Uri uri, CancellationToken cancel) => Execute(GetAsync(uri, cancel));
        public string GetString(string uri) => GetString(uri, CancellationToken.None);
        public string GetString(string uri, CancellationToken cancel) => GetString(new Uri(uri), cancel);
        public string GetString(Uri uri) => GetString(uri, CancellationToken.None);
        public string GetString(Uri uri, CancellationToken cancel) => Execute(GetStringAsync(uri, cancel));
        public HttpResponseMessage Delete(string uri) => Delete(uri, CancellationToken.None);
        public HttpResponseMessage Delete(string uri, CancellationToken cancel) => Delete(new Uri(uri), cancel);
        public HttpResponseMessage Delete(Uri uri) => Delete(uri, CancellationToken.None);
        public HttpResponseMessage Delete(Uri uri, CancellationToken cancel) => Execute(DeleteAsync(uri, cancel));
        public HttpResponseMessage Post(string uri, HttpContent content) => Post(uri, content, CancellationToken.None);
        public HttpResponseMessage Post(string uri, HttpContent content, CancellationToken cancel) => Post(new Uri(uri), content, cancel);
        public HttpResponseMessage Post(Uri uri, HttpContent content) => Post(uri, content, CancellationToken.None);
        public HttpResponseMessage Post(Uri uri, HttpContent content, CancellationToken cancel) => Execute(PostAsync(uri, content, cancel));
        public HttpResponseMessage Put(string uri, HttpContent content) => Put(uri, content, CancellationToken.None);
        public HttpResponseMessage Put(string uri, HttpContent content, CancellationToken cancel) => Put(new Uri(uri), content, cancel);
        public HttpResponseMessage Put(Uri uri, HttpContent content) => Put(uri, content, CancellationToken.None);
        public HttpResponseMessage Put(Uri uri, HttpContent content, CancellationToken cancel) => Execute(PutAsync(uri, content, cancel));
        public HttpResponseMessage Patch(string uri, HttpContent content) => Patch(uri, content, CancellationToken.None);
        public HttpResponseMessage Patch(string uri, HttpContent content, CancellationToken cancel) => Patch(new Uri(uri), content, cancel);
        public HttpResponseMessage Patch(Uri uri, HttpContent content) => Patch(uri, content, CancellationToken.None);
        public HttpResponseMessage Patch(Uri uri, HttpContent content, CancellationToken cancel) => Execute(PatchAsync(uri, content, cancel));

        public abstract User GetUser(string name, CancellationToken cancel);
        public User GetUser(string name) => GetUser(name, CancellationToken.None);
        public async Task<User> GetUserAsync(string name) => await GetUserAsync(name, CancellationToken.None);
        public async Task<User> GetUserAsync(string name, CancellationToken cancel) => await Task.Run(() => GetUser(name, cancel));

        public abstract IEnumerable<Tweet> GetTimeline(User user, CancellationToken cancel);
        public IEnumerable<Tweet> GetTimeline(User user) => GetTimeline(user, CancellationToken.None);
        public async Task<IEnumerable<Tweet>> GetTimelineAsync(User user) => await GetTimelineAsync(user, CancellationToken.None);
        public async Task<IEnumerable<Tweet>> GetTimelineAsync(User user, CancellationToken cancel) => await Task.Run(() => GetTimeline(user, cancel));
        public IEnumerable<Tweet> GetTimeline(string name, CancellationToken cancel) => GetTimeline(GetUser(name, cancel), cancel);
        public IEnumerable<Tweet> GetTimeline(string name) => GetTimeline(name, CancellationToken.None);
        public async Task<IEnumerable<Tweet>> GetTimelineAsync(string name) => await GetTimelineAsync(name, CancellationToken.None);
        public async Task<IEnumerable<Tweet>> GetTimelineAsync(string name, CancellationToken cancel) => await Task.Run(() => GetTimeline(name, cancel));

        /*
                public IEnumerable<Tweet> GetTimeline(string name) => GetTimeline(GetUser(name));
                public abstract IEnumerable<Tweet> GetTimeline(User user); 
                public async Task<IEnumerable<Tweet>> GetTimelineAsync(User user) => await GetTimelineAsync(user, CancellationToken.None);
                public async Task<IEnumerable<Tweet>> GetTimelineAsync(User user, CancellationToken cancel)
                {
                    var ub = new UriBuilder(_base) { Path = "/2/users/" + user.ID + "/tweets", };
                    var response = await GetAsync(ub.Uri, cancel);
                    var json = Json.Load(response.Content.ReadAsStream(cancel));

                    return json["data"].AsArray().Select(_ => new Tweet()
                    {
                        User = user,
                        ID = _["id"].Value,
                        Text = _["text"].Unescape(),

                    });
                }
                public Json GetLikes(string id) => Execute(GetLikesAsync(id));
                public async Task<Json> GetLikesAsync(string id) => await GetLikesAsync(id, CancellationToken.None);
                public async Task<Json> GetLikesAsync(string id, CancellationToken cancel)
                {
                    var url = "https://api.twitter.com/1.1/favorites/list.json?user_id={0}&count=2";
                    var response = await GetAsync(string.Format(url, id), cancel);
                    return Json.Load(response.Content.ReadAsStream(cancel));
                }
                public Json GetShow(string id) => Execute(GetShowAsync(id));
                public async Task<Json> GetShowAsync(string id) => await GetShowAsync(id, CancellationToken.None);
                public async Task<Json> GetShowAsync(string id, CancellationToken cancel)
                {
                    var url = "https://api.twitter.com/1.1/statuses/show.json?id={0}";
                    var response = await GetAsync(string.Format(url, id), cancel);
                    return Json.Load(response.Content.ReadAsStream(cancel));
                }
        */
    }
}
