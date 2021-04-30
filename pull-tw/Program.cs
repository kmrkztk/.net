using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib;
using Lib.Json;
using Lib.Web.Twitter;

namespace pull_tw
{
    class Program
    {
        static void Main()
        {
            using var fs = new FileStream("pull-tw.settings.json", FileMode.Open);
            var settings = Json.Load<Settings>(fs);
            using var client = new TwitterClient(settings.Bearer);
            foreach(var target in settings.Targets) Download(client, target);
            ConsoleEx.Pause();
        }
        static void Download(TwitterClient client, Settings.Target target)
        {
            if (string.IsNullOrEmpty(target.UserName)) return;
            if (!File.Exists(target.SaveTo)) Directory.CreateDirectory(target.SaveTo);
            var user = client.GetUserAsync(target.UserName).Result;
            var option = target.Option;
            do
            {
                var tl = client.GetTimelineAsync(user, option).Result;
                tl.Foreach(_ => target.Save(_));
                option.NextToken = tl.Meta.NextToken;
            }
            while (!string.IsNullOrEmpty(option.NextToken));
        }
    }

    class Settings
    {
        [Mapping("bearer")]     public string Bearer { get; set; }
        [Mapping("access-key")] public string AccessKey { get; set; }
        [Mapping("secret-key")] public string SecretKey { get; set; }
        [Mapping("targets")]
        public List<Target> Targets { get; set; }
        public class Target
        {
            [Mapping("username")]       public string UserName { get; set; }
            [Mapping("userid")]         public string UserID { get; set; }
            [Mapping("tweet-type")]     public List<string> TweetType { get; set; }
            [Mapping("save-content")]   public List<string> Contents { get; set; }
            [Mapping("save-to")]        public string SaveTo { get; set; }

            public bool HasText => Contents.Any(_ => _.ToLower() == "text");
            public bool HasPhoto => Contents.Any(_ => _.ToLower() == "photo");
            public bool HasVideo => Contents.Any(_ => _.ToLower() == "video");
            public bool HasMedia => HasPhoto || HasVideo;
            public bool HasReply => TweetType.Any(_ => _.ToLower() == "reply");
            public bool HasRetweet => TweetType.Any(_ => _.ToLower() == "retweet");
            public bool HasLike => TweetType.Any(_ => _.ToLower() == "like");
            public string NewestId => new DirectoryInfo(SaveTo)
                .GetDirectories()
                .Select(_ => _.Name)
                .OrderByDescending(_ => _, new Comparer())
                .FirstOrDefault();
            public TimelineOption Option => new()
            {
                Expansions = HasMedia ? TimelineOption.ExpansionsOptions.AttachmentsMediaKeys : TimelineOption.ExpansionsOptions.None,
                TweetFields = HasMedia ? TimelineOption.TweetFieldsOptions.Attachments : TimelineOption.TweetFieldsOptions.None,
                MediaFields = HasMedia ? TimelineOption.MediaFieldsOptions.Type | TimelineOption.MediaFieldsOptions.Url : TimelineOption.MediaFieldsOptions.None,
                Exclude =
                (HasReply   ? TimelineOption.ExcludeOptions.None : TimelineOption.ExcludeOptions.Replies) |
                (HasRetweet ? TimelineOption.ExcludeOptions.None : TimelineOption.ExcludeOptions.Retweets) |
                TimelineOption.ExcludeOptions.None,
                SinceId = NewestId,
            };
            public void Save(Tweet tweet)
            {
                Console.WriteLine("saving... [{0}]", tweet.ID);
                var path = new DirectoryInfo(SaveTo).CreateSubdirectory(tweet.ID);
                var regex = new System.Text.RegularExpressions.Regex(@"\?.+$");
                string filepath(string name, string ext) => path.ToString() + "\\" + name + regex.Replace(Path.GetExtension(ext), "");
                if (HasText) File.WriteAllText(filepath(tweet.ID, ".txt"), tweet.Text);
                using var client = new HttpClient();
                tweet.Medias?
                    .Where(_ => _ != null)
                    .Where(_ => (_.IsPhoto && HasPhoto) || (_.IsVideo && HasVideo))
                    .Foreach(_ => client.DownloadAsync(_.Url, filepath(_.ID, _.Url)).Wait());
            }
            class Comparer : IComparer<string>
            {
                public int Compare(string x, string y)
                {
                    if (x.Length < y.Length) return -Compare(y, x);
                    return x.CompareTo(y.PadLeft(x.Length, '0'));
                }
            }
        }
    }
}
