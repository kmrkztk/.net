using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Entity;
using Lib.Json;

namespace Lib.Web.Twitter
{
    public class Tweet
    {
        public ID ID { get; init; }
        public string Text { get; init; }
        public DateTime? CreatedAt { get; init; }
        public ID AuthorID { get; init; }
        public ID ConversationID { get; init; }
        public ID InReplyToUserID { get; init; }
        public List<ReferencedTweet> ReferencedTweets { get; init; }
        public List<Attachment> Attachments { get; init; }
        public List<Media> Medias { get; init; }
        public Tweet() { }
        public Tweet(JsonObject json, Func<string, Media> media)
        {
            ID = json["id"].Value;
            Text = json["text"].Unescape();
            CreatedAt = DateTime.Parse(json["created_at"]?.Value);
            Medias = json["attachments"]?["media_keys"]?.AsArray()
                .Select(_ => _.Value)
                .Select(_ => media(_))
                .ToList();
        }
        public override string ToString() => string.Format("[{0:yyyy/MM/dd HH:mm:ss}] ({1}){2}", CreatedAt, ID, Text);
        public class ReferencedTweet
        {
            public const string Retweeted = "retweeted";
            public const string Quoted = "quoted";
            public const string RepliedTo = "replied_to";
            public string Type { get; init; }
            public ID ID { get; init; }
        }
        public class Attachment
        {
            public List<string> MediaKeys { get; init; }
            public List<ID> PollIDs { get; init; }
        }
        public class Geo
        {
            public string Type { get; init; }
            public List<decimal> Values { get; init; }
            public ID PlaceID { get; init; }
        }
        public class ContextAnnotation
        {
            public class Annotation
            {
                public ID ID { get; init; }
                public string Name { get; init; }
                public string Description { get; init; }
            }
            public Annotation Domain { get; set; }
            public Annotation Entity { get; set; }
        }
        public class Entity
        {
            public class Annotation
            {
                public int? Start { get; set; }
                public int? End { get; set; }
                public decimal? Probability { get; set; }
                public string Type { get; set; }
                public string NormalizedText { get; set; }
            }
            public class Url
            {
                public int? Start { get; set; }
                public int? End { get; set; }
                public string Value { get; set; }
                public string Expanded { get; set; }
                public string Display { get; set; }
                public string Unknownd { get; set; }
            }
            public class HashTag
            {
                public int? Start { get; set; }
                public int? End { get; set; }
                public string Tag { get; set; }
            }
            public class Mention
            {
                public int? Start { get; set; }
                public int? End { get; set; }
                public string UserName { get; set; }
            }
            public class CashTag
            {
                public int? Start { get; set; }
                public int? End { get; set; }
                public string Tag { get; set; }
            }
            public List<Annotation> Annotations { get; set; }
            public List<Url> Urls { get; set; }
            public List<HashTag> HashTags { get; set; }
            public List<Mention> Mentions { get; set; }
            public List<CashTag> CashTags { get; set; }
        }
        public class Media
        {
            public const string Photo = "photo";
            public const string Video = "video";
            public ID ID { get; set; }
            public string Key { get; set; }
            public string Type { get; set; }
            public string Url { get; set; }
            public bool IsPhoto => Type == Photo;
            public bool IsVideo => Type == Video;
            public Media() { }
            public Media(JsonObject json)
            {
                ID = json["media_key"]?.Value.Split("_")[1];
                Key = json["media_key"]?.Value;
                Url = json["url"]?.Value;
                Type = json["type"]?.Value;
            }
            public override string ToString() => string.Format("({0})[{1}]{2}", ID, Type, Url);
            public static IEnumerable<Media> Of(JsonArray json) => json?.Select(_ => new Media(_.AsObject()));
        }
        public class Place
        {
        }
        public class Poll
        {
        }
    }
}
