using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Lib.Web.Twitter
{
    public class TwitterClient : HttpClient
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
            readonly string _format;
            public Url(decimal version, string path) => _format = string.Format("{0}{1}/{2}{3}", Scheme, Domain, version, path);
            public override string ToString() => _format;
            public string ToString(params object[] parameter) => string.Format(_format, parameter);
        }
        public TwitterClient(string bearer) : this(bearer, new HttpClientHandler() { Proxy = DefaultProxy, }, true) { }
        public TwitterClient(string bearer, HttpMessageHandler handler) : this(bearer, handler, false) { }
        public TwitterClient(string bearer, HttpMessageHandler handler, bool disposeHandler) : base(handler, disposeHandler) =>
            DefaultRequestHeaders.Authorization = new("Bearer", bearer);

        public async Task<Json.Json> GetJsonAsync(string url, CancellationToken cancel)
        {
            var json = await HttpClientExtensions.GetJsonAsync(this, url, cancel);
            if (json["errors"] != null) throw new TwitterApiException(json);
            return json;
        }

        public async Task<User> GetUserAsync(string name) => await GetUserAsync(name, CancellationToken.None);
        public async Task<User> GetUserAsync(string name, CancellationToken cancel) => new((await this.GetJsonAsync(Url.GetUser.ToString(name), cancel))?["data"]?.AsObject());
        public async Task<Timeline> GetTimelineAsync(string name) => await GetTimelineAsync(name, CancellationToken.None);
        public async Task<Timeline> GetTimelineAsync(string name, CancellationToken cancel) => await GetTimelineAsync(await GetUserAsync(name, cancel), cancel);
        public async Task<Timeline> GetTimelineAsync(User user) => await GetTimelineAsync(user, CancellationToken.None);
        public async Task<Timeline> GetTimelineAsync(User user, CancellationToken cancel) => await GetTimelineAsync(user, null, cancel);
        public async Task<Timeline> GetTimelineAsync(string name, TimelineOption options) => await GetTimelineAsync(name, options, CancellationToken.None);
        public async Task<Timeline> GetTimelineAsync(string name, TimelineOption options, CancellationToken cancel) => await GetTimelineAsync(await GetUserAsync(name, cancel), options, cancel);
        public async Task<Timeline> GetTimelineAsync(User user, TimelineOption options) => await GetTimelineAsync(user, options, CancellationToken.None);
        public async Task<Timeline> GetTimelineAsync(User user, TimelineOption options, CancellationToken cancel)
        {
            var json = await this.GetJsonAsync(Url.GetTimeline.ToString(user.ID, options), cancel);
            var medias = Tweet.Media.Of(json?["includes"]?["media"]?.AsArray()).ToDictionary(_ => _.Key);
            return new Timeline(json["data"]?
                .AsArray()
                .Select(_ => _.AsObject())
                .Select(_ => new Tweet(_, k =>
                    !medias.ContainsKey(k) ? null :
                    medias[k].IsPhoto ? medias[k] :
                    medias[k].IsVideo ? GetTweetAsync(_["id"].Value, cancel).Result.Medias?.FirstOrDefault() : null)
                ))
            {
                User = user,
                Meta = new(json["meta"]?.AsObject()),
            };
        }
        public async Task<Tweet> GetTweetAsync(string id, CancellationToken cancel)
        {
            var _ = await this.GetJsonAsync(Url.GetTweet.ToString(id, new()), cancel);
            return new()
            {
                ID = _["id"].Value,
                Text = (_["text"] ?? _["full_text"]).Unescape(),
                Medias = _["extended_entities"]?["media"]?.AsArray()
                        .Select(_ => new Tweet.Media()
                        {
                            ID = _["id"].Value,
                            Type = _["type"].Value,
                            Url =
                                _["type"].Value == "photo" ? _["media_url"].Value :
                                _["type"].Value == "video" ? _["video_info"]["variants"]
                                    .AsArray()
                                    .OrderBy(_ => _["bitrate"]?.Value ?? "a")
                                    .FirstOrDefault()?["url"].Value :
                                null,
                        })
                        .Where(_ => _.Url != null)
                        .ToList(),
            };
        }
        /*
            public override User GetUser(string name, CancellationToken cancel)
            {
                var uri = GetUri("/users/show.json?screen_name=" + name);
                var response = Get(uri, cancel);
                var json = Json.Load(response.Content.ReadAsStream(cancel));
                return new()
                {
                    ID = json["id"].Value,
                    Name = name,
                };
            }
            public override IEnumerable<Tweet> GetTimeline(User user, CancellationToken cancel)
            {
                var uri = GetUri("/statuses/user_timeline.json" +
                    "?trim_user=true" +
                    "&exclude_replies=true" +
                    "&include_rts=false" +
                    "&count=20000" +
                    "&tweet_mode=extended" +
                    "&user_id=" + user.ID);
                var response = Get(uri, cancel);
                var json = Json.Load(response.Content.ReadAsStream(cancel));
                return json.AsArray().Select(_ => new Tweet()
                {
                    User = user,
                    ID = _["id"].Value,
                    Text = _["full_text"].Unescape(),
                    Medias = _["extended_entities"]?["media"]?.AsArray()
                        .Select(_ => new Media() {
                            ID = _["id"].Value,
                            Type = _["type"].Value,
                            Url = 
                                _["type"].Value == "photo" ? _["media_url"].Value :
                                _["type"].Value == "video" ? _["video_info"]["variants"][0]["url"].Value :
                                null,
                            })
                        .Where(_ => _.Url != null)
                });;
            }
         */
    }
}
