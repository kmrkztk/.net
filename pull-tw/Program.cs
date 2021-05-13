using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using Lib;
using Lib.Web.Twitter;
using Lib.Web.Twitter.Objects;
using Lib.Entity;

namespace pull_tw
{
    class Program
    {
        static void Main()
        {
            var settings = Settings.Load();
            using var client = new TwitterClient(settings.Bearer);
            settings.Targets
                .Where(_ => !string.IsNullOrEmpty(_.UserName))
                .Foreach(target =>
            {
                Console.WriteLine("download '{0}'...", target.UserName);
                target.CreateSavingTo();
                var option = target.Option;
                var old = ID.Max;
                var count = 0;
                string path(object name, string ext) => string.Format(@"{0}\{1}{2}", target.SaveTo, name, ext);
                void save(Tweet tweet)
                {
                    var text = tweet.Text
                            .Replace("\r", " ")
                            .Replace("\n", " ");
                    Console.WriteLine("saving... tweet [{0}] at {1} '{2}'",
                        tweet.ID,
                        tweet.CreatedAt,
                        text.Length > 20 ? (text[0..20] + "...") : text);
                    File.WriteAllText(path(tweet.ID, ".txt"), tweet.Text);
                }
                void download(Media media)
                {
                    Console.WriteLine("downloading... [{0}] '{1}'", media.ID, media.Url);
                    var regex = new Regex(@"\?.+$");
                    var filename = path(media.ID, Path.GetExtension(regex.Replace(media.Url, "")));
                    using var client = new HttpClient();
                    client.DownloadAsync(media.Url, filename).Wait();
                }
                void meta(Meta meta)
                {
                    Console.WriteLine("next '{0}'", meta.NextToken);
                    if (target.NewestId > meta?.NewestId) return;
                    target.NewestId = meta?.NewestId;
                    var filename = path(target.UserName, ".newest.txt");
                    File.WriteAllText(filename, target.NewestId.ToString());
                }

                do
                {
                    var timeline = client.GetTimelineAsync(target.UserName, option).Result;
                    var medias = timeline.Includes?.Media?.ToDictionary(_ => _.Key) ?? new();
                    timeline
                    .Where(_ =>
                        (target.HasTweet && !_.IsReply && !_.IsRetweet) ||
                        (target.HasReply && _.IsReply) ||
                        (target.HasRetweet && _.IsRetweet))
                    .Foreach(_ =>
                    {
                        if (target.HasText) save(_);
                        _.Attachments?
                            .MediaKeys?
                            .Where(k => medias.ContainsKey(k))
                            .Select(k => medias[k])
                            .Select(m => m.IsVideo ? client.GetTweetAsync(_.ID).Result.Includes?.Media?.FirstOrDefault() : m)
                            .Where(m => (m.IsPhoto && target.HasPhoto) || (m.IsVideo && target.HasVideo))
                            .Foreach(m => download(m));
                    });
                    meta(timeline.Meta);
                    if (timeline.Meta?.OldestId != ID.Null) 
                        old = old < timeline.Meta.OldestId ? old : timeline.Meta.OldestId;
                    count += timeline.Meta?.ResultCount ?? 0;
                    option.NextToken = timeline.Meta?.NextToken;
                    if (string.IsNullOrEmpty(option.NextToken))
                    {
                        if (old == ID.Max) break;
                        if (option.UntilId == old - 1) break;
                        option.UntilId = old - 1;
                    }
                }
                while (true);
                Console.WriteLine("saved {0} tweets.", count);
            });
#if DEBUG
            ConsoleEx.Pause();
#endif
        }
    }
}
