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
    partial class TwitterClient
    {
        public static TwitterClient V2(string bearer) => new TwitterClient2(bearer);
        class TwitterClient2 : TwitterClient
        {
            public TwitterClient2(string bearer) : base(bearer, "2") { }
            public override User GetUser(string name, CancellationToken cancel)
            {
                var uri = GetUri("/users/by/username/" + name);
                var response = Get(uri, cancel);
                var json = Json.Load(response.Content.ReadAsStream(cancel));
                return new()
                {
                    ID = json["data"]["id"].Value,
                    Name = name,
                };
            }
            public override IEnumerable<Tweet> GetTimeline(User user, CancellationToken cancel)
            {
                var uri = GetUri("/users/" + user.ID + "/tweets" +
                    "?tweet.fields=attachments" +
                    "&media.fields=type,url"
                    );
                var response = Get(uri, cancel);
                var json = Json.Load(response.Content.ReadAsStream(cancel));
                Console.WriteLine(json);
                return json["data"].AsArray().Select(_ => new Tweet()
                {
                    User = user,
                    ID = _["id"].Value,
                    Text = _["text"].Unescape(),
                });
            }
        }
    }
}
