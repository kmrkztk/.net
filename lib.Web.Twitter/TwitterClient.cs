using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Lib.Entity;
using Lib.Jsons;
using Lib.Web.Twitter.Objects;
using Lib.Web.Twitter.Options;

namespace Lib.Web.Twitter
{
    public class TwitterClient
    {
        public class Url 
        {
            public const string Scheme = "https://";
            public const string Domain = "api.twitter.com";
            public static Url Ver1(string path) => new(1.1m, path);
            public static Url Ver2(string path) => new(2, path);
            public static Url GetUser     => Ver2("/users/by/username/{0}");
            public static Url GetTimeline => Ver2("/users/{0}/tweets?{1}");
            public static Url GetTimelineVer1 => Ver1("/statuses/user_timeline.json?{0}");
            public static Url GetTweet    => Ver1("/statuses/show.json?{0}");
            public static Url GetFavorite => Ver1("/favorites/list.json?{0}");
            readonly string _format;
            public Url(decimal version, string path) => _format = string.Format("{0}{1}/{2}{3}", Scheme, Domain, version, path);
            public override string ToString() => _format;
            public string ToString(params object[] parameter) => string.Format(_format, parameter);
        }
        readonly HttpClient _client;
        readonly bool _dispose = false;
        public TwitterClient(string bearer) : this(new HttpClient(), bearer) => _dispose = true;
        public TwitterClient(HttpClient client, string bearer)
        {
            _client = client;
            _client.DefaultRequestHeaders.Authorization = new("Bearer", bearer);
        }
        public void Close() => _client.Dispose();
        ~TwitterClient()
        {
            if (_dispose) _client.Dispose();
        }
        public async Task<Json> GetJsonAsync(string url, CancellationToken cancel) => await Task.Run(() => GetJson(url, cancel), cancel);
        public Json GetJson(string url, CancellationToken cancel)
        {
            Console.WriteLine(url);
            var json = _client.GetJson(url, cancel);
            if (json is JsonObject && json["errors"] != null) throw new TwitterApiException(json);
            return json;
        }
        public async Task<User> GetUserAsync(string name) => await GetUserAsync(name, CancellationToken.None);
        public virtual async Task<User> GetUserAsync(string name, CancellationToken cancel) 
            => (await this.GetJsonAsync(Url.GetUser.ToString(name), cancel))?["data"]?.Cast<User>();
        public async Task<Tweets> GetTimelineAsync(string name) => await GetTimelineAsync(name, CancellationToken.None);
        public async Task<Tweets> GetTimelineAsync(string name, CancellationToken cancel) => await GetTimelineAsync(await GetUserAsync(name, cancel), cancel);
        public async Task<Tweets> GetTimelineAsync(User user) => await GetTimelineAsync(user, CancellationToken.None);
        public async Task<Tweets> GetTimelineAsync(User user, CancellationToken cancel) => await GetTimelineAsync(user, null, cancel);
        public async Task<Tweets> GetTimelineAsync(string name, TimelineOption options) => await GetTimelineAsync(name, options, CancellationToken.None);
        public async Task<Tweets> GetTimelineAsync(string name, TimelineOption options, CancellationToken cancel) => await GetTimelineAsync(await GetUserAsync(name, cancel), options, cancel);
        public async Task<Tweets> GetTimelineAsync(User user, TimelineOption options) => await GetTimelineAsync(user, options, CancellationToken.None);
        public async Task<Tweets> GetTimelineAsync(User user, TimelineOption options, CancellationToken cancel) 
            => Tweets.OfTimeline(await GetJsonAsync(Url.GetTimeline.ToString(user.ID, options), cancel), user);
        public async Task<Tweets> GetTimelineAsync(TimelineOption_Ver1 options) => await GetTimelineAsync(options, CancellationToken.None);
        public async Task<Tweets> GetTimelineAsync(TimelineOption_Ver1 options, CancellationToken cancel)
            => Tweets.OfVer1(await GetJsonAsync(Url.GetTimelineVer1.ToString(options), cancel), new User() { ID = options.UserId, UserName = options.ScreenName });
        public async Task<(Tweet Tweet, Includes Includes)> GetTweetAsync(ID id) => await GetTweetAsync(id, CancellationToken.None);
        public async Task<(Tweet Tweet, Includes Includes)> GetTweetAsync(ID id, CancellationToken cancel) => await GetTweetAsync(new TweetOption(id), cancel);
        public async Task<(Tweet Tweet, Includes Includes)> GetTweetAsync(TweetOption options) => await GetTweetAsync(options, CancellationToken.None);
        public async Task<(Tweet Tweet, Includes Includes)> GetTweetAsync(TweetOption options, CancellationToken cancel)
        {
            var json = await this.GetJsonAsync(Url.GetTweet.ToString(options), cancel);
            return (Tweet.OfVer1(json), Includes.OfVer1(json));
        }
        public async Task<Tweets> GetLikesAsync(string name) => await GetLikesAsync(new TimelineOption_Ver1() { ScreenName = name, });
        public async Task<Tweets> GetLikesAsync(TimelineOption_Ver1 options) => await GetLikesAsync(options, CancellationToken.None);
        public async Task<Tweets> GetLikesAsync(TimelineOption_Ver1 options, CancellationToken cancel) 
            => Tweets.OfVer1(await GetJsonAsync(Url.GetFavorite.ToString(options), cancel), new User() { ID = options.UserId, UserName = options.ScreenName, });
    }
}
