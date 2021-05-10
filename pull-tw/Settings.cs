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
using Lib.Json;
using Lib.Web.Twitter;
using Lib.Web.Twitter.Objects;

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
        [ChainCaseName] public List<Target> Targets { get; set; }
        public class Target
        {
            [LowerName] public string UserName { get; set; }
            [LowerName] public string UserID { get; set; }
            [ChainCaseName] public List<string> TweetType { get; set; }
            [ChainCaseName] public List<string> SaveContent { get; set; }
            string _saveTo = null;
            [ChainCaseName] public string SaveTo 
            {
                get => _saveTo ?? @".\" + UserName;
                set => _saveTo = value; 
            }
            DateTime? _starttime = null;
            [ChainCaseName] public string StartTime 
            {
                get => _starttime.GetValueOrDefault().ToString("yyyy/MM/dd HH:mm:ss"); 
                set => _starttime = value == null ? null : DateTime.Parse(value); 
            }
            [ChainCaseName] public bool Refresh { get; set; }

            public bool HasText => SaveContent.Any(_ => _.ToLower() == "text");
            public bool HasPhoto => SaveContent.Any(_ => _.ToLower() == "photo");
            public bool HasVideo => SaveContent.Any(_ => _.ToLower() == "video");
            public bool HasMedia => HasPhoto || HasVideo;
            public bool HasReply => TweetType.Any(_ => _.ToLower() == "reply");
            public bool HasRetweet => TweetType.Any(_ => _.ToLower() == "retweet");
            public bool HasLike => TweetType.Any(_ => _.ToLower() == "like");

            ID? _newest = null;
            public ID NewestId => _newest ??=
                File.Exists(SaveTo + "\\" + UserName + ".newest.txt") ?
                File.ReadAllText(SaveTo + "\\" + UserName + ".newest.txt") : null;

            public TimelineOption Option => new()
            {
                MaxResult = 100,
                Expansions = HasMedia ? ExpansionsOptions.AttachmentsMediaKeys : ExpansionsOptions.None,
                TweetFields = (HasMedia ? TweetFieldsOptions.Attachments : TweetFieldsOptions.None) | TweetFieldsOptions.CreatedAt,
                MediaFields = HasMedia ? MediaFieldsOptions.Type | MediaFieldsOptions.Url : MediaFieldsOptions.None,
                //Exclude =
                //(HasReply ? TimelineOption.ExcludeOptions.None : TimelineOption.ExcludeOptions.Replies) |
                //(HasRetweet ? TimelineOption.ExcludeOptions.None : TimelineOption.ExcludeOptions.Retweets) |
                //TimelineOption.ExcludeOptions.None,
                SinceId = Refresh ? ID.Null : NewestId,
                StartTime = _starttime,
            };
            public void CreateSavingTo() => Directory.CreateDirectory(SaveTo);

            public void Download(Tweet tweet)
            {
                var filename = SaveTo + "\\" + tweet.ID + ".txt";
                File.WriteAllText(filename, tweet.Text);
            }
            public void Download(Media media)
            {
                var regex = new Regex(@"\?.+$");
                var filename = SaveTo + "\\" + media.ID + Path.GetExtension(regex.Replace(media.Url, ""));
                using var client = new HttpClient();
                client.DownloadAsync(media.Url, filename).Wait();
            }
            public void Download(Meta meta)
            {
                if (NewestId > meta?.NewestId) return;
                _newest = meta?.NewestId;
                var filename = SaveTo + "\\" + UserName + ".newest.txt";
                File.WriteAllText(filename, _newest);
            }
        }
    }
}
