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
using Lib;
using Lib.Json;

namespace pull_tw
{
    partial class TwitterClient
    {
        public static TwitterClient V1_1(string bearer) => new TwitterClient1_1(bearer);
        class TwitterClient1_1 : TwitterClient
        {
            public TwitterClient1_1(string bearer) : base(bearer, "1.1") { }
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
        }
    }
}
