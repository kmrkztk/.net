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

namespace Lib.Web.Twitter
{
    public class TwitterClient : HttpClient
    {
        const string URL = "https://api.twitter.com";

        public TwitterClient(string bearer) : this(bearer, new HttpClientHandler() { Proxy = DefaultProxy, }, true) { }
        public TwitterClient(string bearer, HttpMessageHandler handler) : this(bearer, handler, false) { }
        public TwitterClient(string bearer, HttpMessageHandler handler, bool disposeHandler) : base(handler, disposeHandler) =>
            DefaultRequestHeaders.Authorization = new("Bearer", bearer);

        public async Task<User> GetUserAsync(string name) => await GetUserAsync(name, CancellationToken.None);
        public async Task<User> GetUserAsync(string name, CancellationToken cancel) 
        {
            var uri = URL + "/2/users/by/username/" + name;
            var json = await this.GetJsonAsync(uri, cancel);
            return new()
            {
                ID = json["data"]["id"].Value,
                Name = name,
            };
        }

        public async Task<Timeline> GetTimelineAsync(string name) => await GetTimelineAsync(name, CancellationToken.None);
        public async Task<Timeline> GetTimelineAsync(string name, CancellationToken cancel) => await GetTimelineAsync(await GetUserAsync(name, cancel), cancel);
        public async Task<Timeline> GetTimelineAsync(User user) => await GetTimelineAsync(user, CancellationToken.None);
        public async Task<Timeline> GetTimelineAsync(User user, CancellationToken cancel) => await GetTimelineAsync(user, null, cancel);
        public async Task<Timeline> GetTimelineAsync(string name, TimelineOption options) => await GetTimelineAsync(name, options, CancellationToken.None);
        public async Task<Timeline> GetTimelineAsync(string name, TimelineOption options, CancellationToken cancel) => await GetTimelineAsync(await GetUserAsync(name, cancel), options, cancel);
        public async Task<Timeline> GetTimelineAsync(User user, TimelineOption options) => await GetTimelineAsync(user, options, CancellationToken.None);
        public async Task<Timeline> GetTimelineAsync(User user, TimelineOption options, CancellationToken cancel)
        {
            var uri = URL + "/2/users/" + user.ID + "/tweets" + "?" + options?.ToString();
            var json = await this.GetJsonAsync(uri, cancel);
            var medias = json["includes"]["media"].AsArray()
                .ToDictionary(
                _ => _["media_key"].Value, 
                _ => new Media()
                    {
                        ID = _["media_key"].Value,
                        Url = _["url"]?.Value,
                        Type = _["type"].Value,
                    });
            return new Timeline(json["data"].AsArray().Select(_ => new Tweet()
            {
                User = user,
                ID = _["id"].Value,
                Text = _["text"].Unescape(),
                Medias = _["attachments"]?["media_keys"]?.AsArray()
                    .Select(__ =>
                        medias[__.Value].Type == "photo" ?
                        medias[__.Value] :
                        GetTweetAsync(_["id"].Value, cancel).Result.Medias.FirstOrDefault()
                        ),
            }))
            {
                Meta = new()
                {
                    NextToken = json["meta"]["next_token"]?.Value,
                    NewestId = json["meta"]["newest_id"]?.Value,
                    OldestId = json["meta"]["oldest_id"]?.Value,
                    ResultCount = int.Parse(json["meta"]["result_count"]?.Value),
                }
            };
            //return json["data"].AsArray().Select(_ => GetTweetAsync(_["id"].Value, cancel).Result);
        }
        public async Task<Tweet> GetTweetAsync(string id, CancellationToken cancel)
        {
            var uri = URL + "/1.1/statuses/show.json" +
                    "?trim_user=true" +
                    "&include_my_retweet=false" +
                    "&include_entities=true" +
                    "&tweet_mode=extended" +
                    "&id=" + id;
            var _ = await this.GetJsonAsync(uri, cancel);
            return new()
            {
                ID = _["id"].Value,
                Text = _["full_text"].Unescape(),
                Medias = _["extended_entities"]?["media"]?.AsArray()
                        .Select(_ => new Media()
                        {
                            ID = _["id"].Value,
                            Type = _["type"].Value,
                            Url =
                                _["type"].Value == "photo" ? _["media_url"].Value :
                                _["type"].Value == "video" ? _["video_info"]["variants"].AsArray().OrderBy(_ => _["bitrate"]?.Value ?? "a").FirstOrDefault()?["url"].Value :
                                null,
                        })
                        .Where(_ => _.Url != null)
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
