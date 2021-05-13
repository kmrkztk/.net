using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Web.Twitter.Options
{
    [Flags]
    public enum ExpansionsOptions
    {
        [Name(null)]                             None                        = 0x00,
        [Name("attachments.poll_ids")]           AttachmentsPollIds          = 0x01,
        [Name("attachments.media_keys")]         AttachmentsMediaKeys        = 0x02,
        [Name("author_id")]                      AuthorId                    = 0x04,
        [Name("entities.mentions.username")]     EntitiesMentionsUsername    = 0x08,
        [Name("geo.place_id")]                   GeoPlaceId                  = 0x10,
        [Name("in_reply_to_user_id")]            InReplyToUserId             = 0x20,
        [Name("referenced_tweets.id")]           ReferencedTweetsId          = 0x40,
        [Name("referenced_tweets.id.author_id")] referencedTweetsIdAuthorId  = 0x80,
    }
    [Flags]
    public enum TweetFieldsOptions
    {
        [Name(null)]    None               = 0x00,
        [SnakeCaseName] Attachments        = 0x01,
        [SnakeCaseName] AuthorId           = 0x02,
        [SnakeCaseName] ContextAnnotations = 0x04,
        [SnakeCaseName] ConversationId     = 0x08,
        [SnakeCaseName] CreatedAt          = 0x10,
        [SnakeCaseName] Entities           = 0x20,
        [SnakeCaseName] Geo                = 0x40,
        [SnakeCaseName] Id                 = 0x80,
        [SnakeCaseName] InReplyToUserId    = 0x0100,
        [SnakeCaseName] Lang               = 0x0200,
        [SnakeCaseName] NonPublicMetrics   = 0x0400,
        [SnakeCaseName] PublicMetrics      = 0x0800,
        [SnakeCaseName] OrganicMetrics     = 0x1000,
        [SnakeCaseName] PromotedMetrics    = 0x2000,
        [SnakeCaseName] PossiblySensitive  = 0x4000,
        [SnakeCaseName] ReferencedTweets   = 0x8000,
        [SnakeCaseName] ReplySettings      = 0x010000,
        [SnakeCaseName] Source             = 0x020000,
        [SnakeCaseName] Text               = 0x040000,
        [SnakeCaseName] Withheld           = 0x080000,
    }
    [Flags]
    public enum MediaFieldsOptions
    {
        [Name(null)]    None               = 0x00,
        [SnakeCaseName] MediaKey           = 0x01,    
        [SnakeCaseName] Type               = 0x02,
        [SnakeCaseName] Url                = 0x04,
        [SnakeCaseName] DurationMs         = 0x08,
        [SnakeCaseName] PreviewImageUrl    = 0x10,
        [SnakeCaseName] Height             = 0x20,
        [SnakeCaseName] Width              = 0x40,
        [SnakeCaseName] PublicMetrics      = 0x80,
        [SnakeCaseName] NonPublicMetrics   = 0x100,
        [SnakeCaseName] OrganicMetrics     = 0x200,
        [SnakeCaseName] PromotedMetrics    = 0x400,
    }
    [Flags]
    public enum PlaceFieldsOptions
    {
        [Name(null)]    None            = 0x00,
        [SnakeCaseName] ContainedWithin = 0x01,
        [SnakeCaseName] Country         = 0x02,
        [SnakeCaseName] CountryCode     = 0x04,
        [SnakeCaseName] FullName        = 0x08,
        [SnakeCaseName] Geo             = 0x10,
        [SnakeCaseName] Id              = 0x20,
        [SnakeCaseName] Name            = 0x40,
        [SnakeCaseName] PlaceType       = 0x80,
    }
    [Flags]
    public enum PollFieldsOptions
    {
        [Name(null)]    None            = 0x00,
        [SnakeCaseName] DurationMinutes = 0x01,
        [SnakeCaseName] EndDatetime     = 0x02,
        [SnakeCaseName] Id              = 0x04,
        [SnakeCaseName] Options         = 0x08,
        [SnakeCaseName] VotingStatus    = 0x10,
    }                                               
    [Flags]
    public enum UserFieldsOptions
    {
        [Name(null)]    None            = 0x00,
        [SnakeCaseName] CreatedAt       = 0x01,
        [SnakeCaseName] Description     = 0x02,
        [SnakeCaseName] Entities        = 0x04,
        [SnakeCaseName] Id              = 0x08,
        [SnakeCaseName] Location        = 0x10,
        [SnakeCaseName] Name            = 0x20,
        [SnakeCaseName] PinnedTweetId   = 0x40,
        [SnakeCaseName] ProfileImageUrl = 0x80,
        [SnakeCaseName] Protected       = 0x0100,
        [SnakeCaseName] PublicMetrics   = 0x0200,
        [SnakeCaseName] Url             = 0x0400,
        [SnakeCaseName] Username        = 0x0800,
        [SnakeCaseName] Verified        = 0x1000,
        [SnakeCaseName] Withheld        = 0x2000,
    }
    [Flags]
    public enum ExcludeOptions
    {
        [Name(null)]    None = 0x00,
        [SnakeCaseName] Replies = 0x01,
        [SnakeCaseName] Retweets = 0x02,
    }
    [Flags]
    public enum TweetModes
    {
        [Name(null)] None = 0x00,
        [SnakeCaseName] Compat = 0x01,
        [SnakeCaseName] Extended = 0x02,
    }
}
