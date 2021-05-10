using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Entity;
using Lib.Json;

namespace Lib.Web.Twitter.Objects
{
    public class Tweet
    {
        [LowerName] public ID ID { get; set; }
        [SnakeCaseName] public string Text { get; set; }
        [SnakeCaseName] public DateTime? CreatedAt { get; set; }
        [SnakeCaseName] public string Source { get; set; }
        [SnakeCaseName] public ID AuthorId { get; set; }
        [SnakeCaseName] public ID ConversationId { get; set; }
        [SnakeCaseName] public ID InReplyToUserId { get; set; }
        [SnakeCaseName] public List<ReferencedTweet> ReferencedTweets { get; set; }
        [SnakeCaseName] public Attachment Attachments { get; set; }
        [SnakeCaseName] public Entities Entities { get; set; }
        [SnakeCaseName] public Geophysics Geo { get; set; }
        [SnakeCaseName] public List<ContextAnnotation> ContextAnnotations { get; set; }
        [SnakeCaseName] public Metrics PublicMetrics { get; set; }
        [SnakeCaseName] public Metrics NonPublicMetrics { get; set; }
        [SnakeCaseName] public Metrics OrganicMetrics { get; set; }
        [SnakeCaseName] public Metrics PromotedMetrics { get; set; }
        [SnakeCaseName] public string ReplySetting { get; set; }
        [SnakeCaseName] public string Lang { get; set; }
        [SnakeCaseName] public bool PossiblySensitive { get; set; }
        [SnakeCaseName] public Withheld Withheld { get; set; }
        public override string ToString() => string.Format("[{0:yyyy/MM/dd HH:mm:ss}] ({1}){2}", CreatedAt, ID, Text);
        public static Tweet OfVer1(JsonObject json) => new()
        {
            ID = json["id"].Value,
            Text = (json["text"] ?? json["full_text"]).Unescape(),
        };

        public class Attachment
        {
            [SnakeCaseName] public List<string> MediaKeys { get; set; }
            [SnakeCaseName] public List<ID> PollIds { get; set; }
        }
        public class Geophysics
        {
            public class Coordinate
            {
                [LowerName] public string Type { get; set; }
                [LowerName] public List<decimal> Coordinates { get; set; }
            }
            [SnakeCaseName] public Coordinate Coordinates { get; set; }
            [SnakeCaseName] public ID PlaceId { get; set; }
        }
        public class ContextAnnotation
        {
            public class Annotation
            {
                [LowerName] public ID ID { get; set; }
                [LowerName] public string Name { get; set; }
                [LowerName] public string Description { get; set; }
            }
            [LowerName] public Annotation Domain { get; set; }
            [LowerName] public Annotation Entity { get; set; }
        }
        public class ReferencedTweet
        {
            public const string Retweeted = "retweeted";
            public const string Quoted = "quoted";
            public const string RepliedTo = "replied_to";
            [LowerName] public string Type { get; set; }
            [LowerName] public ID ID { get; set; }
        }
        public class Metrics
        {
            [SnakeCaseName] public int ImpressionCount { get; set; }
            [SnakeCaseName] public int UrlLinkClicks { get; set; }
            [SnakeCaseName] public int UserProfileClicks { get; set; }
            [SnakeCaseName] public int RetweetCount { get; set; }
            [SnakeCaseName] public int ReplyCount { get; set; }
            [SnakeCaseName] public int LikeCount { get; set; }
            [SnakeCaseName] public int QuoteCount { get; set; }
        }
    }
}
