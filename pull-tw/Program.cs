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
            foreach(var target in settings.Targets.Where(_ => !string.IsNullOrEmpty(_.UserName))) 
                Download(client, "Twitter");
            ConsoleEx.Pause();
        }
        static void Download(TwitterClient client, string username)
        {
            var regex = new System.Text.RegularExpressions.Regex(@"\?.+$");
            var option = new TimelineOption() 
            {
                MaxResult = 100,
                Expansions = TimelineOption.ExpansionsOptions.AttachmentsMediaKeys,
                TweetFields = TimelineOption.TweetFieldsOptions.Attachments,
                MediaFields = TimelineOption.MediaFieldsOptions.Type | TimelineOption.MediaFieldsOptions.Url,
                Exclude = TimelineOption.ExcludeOptions.Replies | TimelineOption.ExcludeOptions.Retweets,
            };
            Console.WriteLine(option);
            var dirname = @".\" + username;
            if (!File.Exists(dirname)) Directory.CreateDirectory(@".\" + dirname);
            var user = client.GetUserAsync(username).Result;
            do
            {
                var tl = client.GetTimelineAsync(user, option).Result;
                var dl = new HttpClient();
                foreach (var t in tl)
                    foreach (var m in t.Medias?.Where(_ => _.Url != null) ?? Enumerable.Empty<Media>()) 
                        dl.DownloadAsync(m.Url, dirname + @"\" + m.ID + regex.Replace(Path.GetExtension(m.Url), "")).Wait();
                if (!tl.Any()) break;
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
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("bearer : {0}", Bearer);
            sb.AppendLine();
            foreach (var t in Targets)
            {
                sb.AppendLine();
                sb.AppendLine("targets[" + Targets.IndexOf(t) + "]");
                sb.AppendLine(t.ToString());
            }
            return sb.ToString();
        }
        public class Target
        {
            [Mapping("username")]       public string UserName { get; set; }
            [Mapping("userid")]         public string UserID { get; set; }
            [Mapping("tweet-type")]     public List<string> TweetType { get; set; }
            [Mapping("save-content")]   public List<string> Contents { get; set; }
            [Mapping("save-to")]        public string SaveTo { get; set; }
            public override string ToString()
            {
                var sb = new StringBuilder();
                sb.AppendFormat("username     : {0}", UserName); sb.AppendLine();
                sb.AppendFormat("userid       : {0}", UserID); sb.AppendLine();
                sb.AppendFormat("tweet-type   : {0}", string.Join(",", TweetType ?? new() { })); sb.AppendLine();
                sb.AppendFormat("save-content : {0}", string.Join(",", Contents ?? new() { })); sb.AppendLine();
                sb.AppendFormat("save-to      : {0}", SaveTo);
                return sb.ToString();
            }
        }
    }
}
