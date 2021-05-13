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
using Lib.Web.Twitter.Objects;

namespace Lib.Web.Twitter
{
    public class TwitterClient : IDisposable
    {
        public class Url 
        {
            public const string Scheme = "https://";
            public const string Domain = "api.twitter.com";
            public static Url Ver1(string path) => new(1.1m, path);
            public static Url Ver2(string path) => new(2, path);
            public static Url GetUser     => Ver2("/users/by/username/{0}");
            public static Url GetTimeline => Ver2("/users/{0}/tweets?{1}");
            public static Url GetTweet    => Ver1("/statuses/show.json?id={0}&{1}");
            public static Url GetFavorite => Ver1("/favorites/list.json?{0}");
            readonly string _format;
            public Url(decimal version, string path) => _format = string.Format("{0}{1}/{2}{3}", Scheme, Domain, version, path);
            public override string ToString() => _format;
            public string ToString(params object[] parameter) => string.Format(_format, parameter);
        }
        readonly HttpClient _client;
        public TwitterClient(string bearer) : this(new HttpClient(), bearer) { }
        public TwitterClient(HttpClient client, string bearer)
        {
            _client = client;
            _client.DefaultRequestHeaders.Authorization = new("Bearer", bearer);
        }
        public void Dispose() => _client.Dispose();
        public async Task<Json> GetJsonAsync(string url, CancellationToken cancel) => await Task.Run(() => GetJson(url, cancel), cancel);
        public Json GetJson(string url, CancellationToken cancel)
        {
            var json = _client.GetJson(url, cancel);
            if (json["errors"] != null) throw new TwitterApiException(json);
            return json;
        }
        public async Task<User> GetUserAsync(string name) => await GetUserAsync(name, CancellationToken.None);
        public virtual async Task<User> GetUserAsync(string name, CancellationToken cancel) => new((await this.GetJsonAsync(Url.GetUser.ToString(name), cancel))?["data"]?.AsObject());
        public async Task<Timeline> GetTimelineAsync(string name) => await GetTimelineAsync(name, CancellationToken.None);
        public async Task<Timeline> GetTimelineAsync(string name, CancellationToken cancel) => await GetTimelineAsync(await GetUserAsync(name, cancel), cancel);
        public async Task<Timeline> GetTimelineAsync(User user) => await GetTimelineAsync(user, CancellationToken.None);
        public async Task<Timeline> GetTimelineAsync(User user, CancellationToken cancel) => await GetTimelineAsync(user, null, cancel);
        public async Task<Timeline> GetTimelineAsync(string name, TimelineOption options) => await GetTimelineAsync(name, options, CancellationToken.None);
        public async Task<Timeline> GetTimelineAsync(string name, TimelineOption options, CancellationToken cancel) => await GetTimelineAsync(await GetUserAsync(name, cancel), options, cancel);
        public async Task<Timeline> GetTimelineAsync(User user, TimelineOption options) => await GetTimelineAsync(user, options, CancellationToken.None);
        public async Task<Timeline> GetTimelineAsync(User user, TimelineOption options, CancellationToken cancel) => new((await this.GetJsonAsync(Url.GetTimeline.ToString(user.Id, options), cancel)).AsObject()) { User = user, };
        public async Task<(Tweet Tweet, Includes Includes)> GetTweetAsync(string id) => await GetTweetAsync(id, CancellationToken.None);
        public async Task<(Tweet Tweet, Includes Includes)> GetTweetAsync(string id, CancellationToken cancel)
        {
            var json = await this.GetJsonAsync(Url.GetTweet.ToString(id, new TweetOption()), cancel);
            return (Tweet.OfVer1(json.AsObject()), Includes.OfVer1(json.AsObject()));
        }
        public async Task<Json> GetLikes(FavoritesOption options, CancellationToken cancel) => await GetJsonAsync(Url.GetFavorite.ToString(options), cancel);
    }
    public class FavoritesOption
    {
        [SnakeCaseName] public string UserId { get; set; }
        [SnakeCaseName] public string ScreenName { get; set; }
        [SnakeCaseName] public int? Count { get; set; }
        [SnakeCaseName] public Lib.Entity.ID? SinceId { get; set; }
        [SnakeCaseName] public Lib.Entity.ID? MaxId { get; set; }
        [SnakeCaseName] public bool? IncludeEntities { get; set; }
    }
}
