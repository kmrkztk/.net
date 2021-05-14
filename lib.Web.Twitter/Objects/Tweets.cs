using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Entity;
using Lib.Jsons;

namespace Lib.Web.Twitter.Objects
{
    public class Tweets : IEnumerable<Tweet>
    {
        readonly List<Tweet> _tweets;
        public Tweet this[int index] { get => _tweets[index]; set => _tweets[index] = value; }
        public User User { get; init; }
        public Includes Includes { get; init; }
        public Meta Meta { get; init; }

        public Tweets(IEnumerable<Tweet> tweets) => _tweets = tweets?.ToList() ?? new();
        public IEnumerator<Tweet> GetEnumerator() => _tweets.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public static Tweets OfTimeline(Json json) => OfTimeline(json, null);
        public static Tweets OfTimeline(Json json, User user)
        {
            if (json == null) return null;
            var tweets = new Tweets(json["data"]?.AsArray().Select(_ => _.Cast<Tweet>()))
            {
                User = user,
                Includes = json["includes"]?.Cast<Includes>(),
                Meta = json["meta"]?.Cast<Meta>(),
            };
            var medias = tweets.Includes?.Media?.ToDictionary(_ => _.Key);
            tweets.Foreach(_ => _.Medias = _
                .Attachments?
                .MediaKeys?
                .Where(_ => medias.ContainsKey(_))
                .Select(_ => medias[_])
                .ToList());
            return tweets;
        }
        public static Tweets OfVer1(Json json) => OfVer1(json, null);
        public static Tweets OfVer1(Json json, User user) 
        {
            var tweets = json.AsArray().Select(_ => Tweet.OfVer1(_)).ToList();
            return new(tweets)
            {
                User = user,
                Includes = Includes.OfArrayVer1(json),
                Meta = new()
                {
                    NewestId = tweets.Count == 0 ? null : tweets.Select(_ => _.ID.ValueOrZero).DefaultIfEmpty().Max(),
                    OldestId = tweets.Count == 0 ? null : tweets.Select(_ => _.ID.ValueOrZero).DefaultIfEmpty().Min(),
                    ResultCount = tweets.Count,
                },
            };
        }
    }
}
