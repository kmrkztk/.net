﻿using System;
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

namespace pull_tw
{

    class Settings
    {
        public static Settings Load()
        {
            using var fs = new FileStream("pull-tw.settings.json", FileMode.Open);
            return Json.Load<Settings>(fs);
        }
        [Mapping("bearer")] public string Bearer { get; set; }
        [Mapping("access-key")] public string AccessKey { get; set; }
        [Mapping("secret-key")] public string SecretKey { get; set; }
        [Mapping("targets")]
        public List<Target> Targets { get; set; }
        public class Target
        {
            [Mapping("username")] public string UserName { get; set; }
            [Mapping("userid")] public string UserID { get; set; }
            [Mapping("tweet-type")] public List<string> TweetType { get; set; }
            [Mapping("save-content")] public List<string> Contents { get; set; }
            [Mapping("save-to")] public string SaveTo { get; set; }

            public bool HasText => Contents.Any(_ => _.ToLower() == "text");
            public bool HasPhoto => Contents.Any(_ => _.ToLower() == "photo");
            public bool HasVideo => Contents.Any(_ => _.ToLower() == "video");
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
                Expansions = HasMedia ? TimelineOption.ExpansionsOptions.AttachmentsMediaKeys : TimelineOption.ExpansionsOptions.None,
                TweetFields =
                (HasMedia ? TimelineOption.TweetFieldsOptions.Attachments : TimelineOption.TweetFieldsOptions.None) |
                TimelineOption.TweetFieldsOptions.CreatedAt,
                MediaFields = HasMedia ? TimelineOption.MediaFieldsOptions.Type | TimelineOption.MediaFieldsOptions.Url : TimelineOption.MediaFieldsOptions.None,
                Exclude =
                (HasReply ? TimelineOption.ExcludeOptions.None : TimelineOption.ExcludeOptions.Replies) |
                (HasRetweet ? TimelineOption.ExcludeOptions.None : TimelineOption.ExcludeOptions.Retweets) |
                TimelineOption.ExcludeOptions.None,
                SinceId = NewestId,
            };
            public void CreateSavingTo() => Directory.CreateDirectory(SaveTo);
            public void Download(Timeline timeline)
            {
                timeline.Foreach(_ =>
                {
                    Console.WriteLine("saving... [{0}] {1}", _.ID, _.CreatedAt);
                    if (HasText) Download(_);
                    _.Medias?
                        .Where(_ => _ != null)
                        .Where(_ => (_.IsPhoto && HasPhoto) || (_.IsVideo && HasVideo))
                        .Foreach(_ => Download(_));
                });
                Download(timeline.Meta);
            }
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