using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lib;
using Lib.Web.Twitter;

namespace pull_tw
{
    class Program
    {
        const string bearer =
            @"AAAAAAAAAAAAAAAAAAAAAE" +
            @"vyOgEAAAAALs4sGxcBrM2X" +
            @"Sciu7jLTT03nCNU%3DgSWs" +
            @"sI9KOcXOOY7BKdhSL0pCfk" +
            @"njp6F4lZOPbwuLoU7ZNxVl8D";

        static void Main()
        {
            using var client = new TwitterClient(bearer);
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
}
