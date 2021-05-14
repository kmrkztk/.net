using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Lib;
using Lib.Entity;
using Lib.Jsons;
using Lib.Web.Twitter;
using Lib.Web.Twitter.Objects;
using Lib.Web.Twitter.Options;

namespace pull_tw
{
    class Settings
    {
        public static Settings Load()
        {
            using var fs = new FileStream("pull-tw.settings.json", FileMode.Open);
            return Json.Load<Settings>(fs);
        }
        [ChainCaseName] public string Bearer { get; set; }
        [ChainCaseName] public string AccessKey { get; set; }
        [ChainCaseName] public string SecretKey { get; set; }
        [ChainCaseName] public int Retry { get; set; }
        [ChainCaseName] public List<Target> Targets { get; set; }
        public class Target
        {
            [LowerName] public string UserName { get; set; }
            [LowerName] public string UserID { get; set; }
            [LowerName] public string Kind { get; set; }
            [ChainCaseName] public List<string> TweetType { get; set; } = new() { "tweet", };
            [ChainCaseName] public List<string> SaveContent { get; set; } = new() { "text", };
            string _saveTo = null;
            [ChainCaseName] public string SaveTo 
            {
                get => _saveTo ?? @".\" + UserName + (string.IsNullOrEmpty(Kind) ? "" : ("." + Kind));
                set => _saveTo = value; 
            }
            DateTime? _starttime = null;
            [ChainCaseName] public string StartTime 
            {
                get => _starttime.GetValueOrDefault().ToString("yyyy/MM/dd HH:mm:ss"); 
                set => _starttime = value == null ? null : DateTime.Parse(value); 
            }
            [ChainCaseName] public bool Refresh { get; set; }

            public bool HasSaveContent(string type) => SaveContent.Any(_ => _.ToLower() == type);
            public bool HasText => HasSaveContent("text");
            public bool HasPhoto => HasSaveContent("photo");
            public bool HasVideo => HasSaveContent("video");
            public bool HasGif => HasSaveContent("gif");
            public bool HasMedia => HasPhoto || HasVideo || HasGif;
            
            public bool HasType(string type) => TweetType.Any(_ => _.ToLower() == type);
            public bool HasTweet => HasType("tweet");
            public bool HasReply => HasType("reply");
            public bool HasRetweet => HasType("retweet");

            public bool IsTimeline => string.IsNullOrEmpty(Kind) || Kind.ToLower() == "timeline";
            public bool IsLikes => Kind?.ToLower() == "likes";
            public bool IsMessages => Kind?.ToLower() == "messages";

            ID? _newest = null;
            public ID? NewestId
            {
                get => _newest ??=
                    File.Exists(SaveTo + "\\" + UserName + ".newest.txt") ?
                    File.ReadAllText(SaveTo + "\\" + UserName + ".newest.txt") : null;
                set => _newest = value;
            }
            public INextOption Option =>
                IsTimeline ? 
                new TimelineOption()
                {
                    MaxResults = 100,
                    Expansions = HasMedia ? ExpansionsOptions.AttachmentsMediaKeys : ExpansionsOptions.None,
                    TweetFields = (HasMedia ? TweetFieldsOptions.Attachments : TweetFieldsOptions.None) 
                        | TweetFieldsOptions.InReplyToUserId
                        | TweetFieldsOptions.ReplySettings
                        | TweetFieldsOptions.ReferencedTweets
                        | TweetFieldsOptions.CreatedAt,
                    MediaFields = HasMedia ? MediaFieldsOptions.Type | MediaFieldsOptions.Url : MediaFieldsOptions.None,
                    SinceId = Refresh ? ID.Null : NewestId,
                    StartTime = _starttime,
                } : 
                IsLikes ?
                new TimelineOption_Ver1()
                {
                    Count = 100,
                    IncludeEntities = true,
                    SinceId = Refresh ? ID.Null : NewestId,
                    TweetMode = TweetModes.Extended,
                    ScreenName = UserName,
                } :
                null;
        }
    }
}
