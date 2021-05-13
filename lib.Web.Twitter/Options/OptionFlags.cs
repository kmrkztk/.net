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
        [Name(null)]                  None               = 0x00,
        [Name("attachments")]         Attachments        = 0x01,
        [Name("author_id")]           AuthorId           = 0x02,
        [Name("context_annotations")] ContextAnnotations = 0x04,
        [Name("conversation_id")]     ConversationId     = 0x08,
        [Name("created_at")]          CreatedAt          = 0x10,
        [Name("entities")]            Entities           = 0x20,
        [Name("geo")]                 Geo                = 0x40,
        [Name("id")]                  Id                 = 0x80,
        [Name("in_reply_to_user_id")] InReplyToUserId    = 0x0100,
        [Name("lang")]                Lang               = 0x0200,
        [Name("non_public_metrics")]  NonPublicMetrics   = 0x0400,
        [Name("public_metrics")]      PublicMetrics      = 0x0800,
        [Name("organic_metrics")]     OrganicMetrics     = 0x1000,
        [Name("promoted_metrics")]    PromotedMetrics    = 0x2000,
        [Name("possibly_sensitive")]  PossiblySensitive  = 0x4000,
        [Name("referenced_tweets")]   ReferencedTweets   = 0x8000,
        [Name("reply_settings")]      ReplySettings      = 0x010000,
        [Name("source")]              Source             = 0x020000,
        [Name("text")]                Text               = 0x040000,
        [Name("withheld")]            Withheld           = 0x080000,
    }
    [Flags]
    public enum MediaFieldsOptions
    {
        [Name(null)]                 None               = 0x00,
        [Name("media_key")]          MediaKey           = 0x01,    
        [Name("type")]               Type               = 0x02,
        [Name("url")]                Url                = 0x04,
        [Name("duration_ms")]        DurationMs         = 0x08,
        [Name("preview_image_url")]  PreviewImageUrl    = 0x10,
        [Name("height")]             Height             = 0x20,
        [Name("width")]              Width              = 0x40,
        [Name("public_metrics")]     PublicMetrics      = 0x80,
        [Name("non_public_metrics")] NonPublicMetrics   = 0x100,
        [Name("organic_metrics")]    OrganicMetrics     = 0x200,
        [Name("promoted_metrics")]   PromotedMetrics    = 0x400,
    }
    [Flags]
    public enum PlaceFieldsOptions
    {
        [Name(null)]                None            = 0x00,
        [Name("contained_within")]  ContainedWithin = 0x01,
        [Name("country")]           Country         = 0x02,
        [Name("country_code")]      CountryCode     = 0x04,
        [Name("full_name")]         FullName        = 0x08,
        [Name("geo")]               Geo             = 0x10,
        [Name("id")]                Id              = 0x20,
        [Name("name")]              Name            = 0x40,
        [Name("place_type")]        PlaceType       = 0x80,
    }
    [Flags]
    public enum PollFieldsOptions
    {
        [Name(null)]                None            = 0x00,
        [Name("duration_minutes")]  DurationMinutes = 0x01,
        [Name("end_datetime")]      EndDatetime     = 0x02,
        [Name("id")]                Id              = 0x04,
        [Name("options")]           Options         = 0x08,
        [Name("voting_status")]     VotingStatus    = 0x10,
    }                                               
    [Flags]
    public enum UserFieldsOptions
    {
        [Name(null)]                None            = 0x00,
        [Name("created_at")]        CreatedAt       = 0x01,
        [Name("description")]       Description     = 0x02,
        [Name("entities")]          Entities        = 0x04,
        [Name("id")]                Id              = 0x08,
        [Name("location")]          Location        = 0x10,
        [Name("name")]              Name            = 0x20,
        [Name("pinned_tweet_id")]   PinnedTweetId   = 0x40,
        [Name("profile_image_url")] ProfileImageUrl = 0x80,
        [Name("protected")]         Protected       = 0x0100,
        [Name("public_metrics")]    PublicMetrics   = 0x0200,
        [Name("url")]               Url             = 0x0400,
        [Name("username")]          Username        = 0x0800,
        [Name("verified")]          Verified        = 0x1000,
        [Name("withheld")]          Withheld        = 0x2000,
    }
    [Flags]
    public enum ExcludeOptions
    {
        [Name(null)] None = 0x00,
        [Name("replies")] Replies = 0x01,
        [Name("retweets")] Retweets = 0x02,
    }
}
