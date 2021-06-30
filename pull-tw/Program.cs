using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lib;
using Lib.Web.Twitter;
using Lib.Web.Twitter.Objects;
using Lib.Web.Twitter.Options;
using Lib.Entity;
using Lib.IO;
using Lib.Logs;

namespace pull_tw
{
    class Program
    {
        static void Main()
        {
            var settings = Settings.Load();
            using var client = new HttpClient();
            var twitter = new TwitterClient(client, settings.Bearer);
            settings.Targets
                .Where(_ => !string.IsNullOrEmpty(_.UserName))
                .Foreach(target =>
            {
                Log.Info().Out("pull '{0}'...", target.UserName);
                string path(object name, string ext) => string.Format(@"{0}\{1}{2}", target.SaveTo, name, ext);
                Directory.CreateDirectory(target.SaveTo);
                var option = target.Option;
                var count = 0;
                var retry = 0;
                var user = twitter.GetUserAsync(target.UserName).Result;
                do
                {
                    var tweets =
                        target.IsTimeline ? twitter.GetTimelineAsync(user, (TimelineOption)option).Result :
                        target.IsLikes ? twitter.GetLikesAsync((TimelineOption_Ver1)option).Result :
                        null;
                    var meta = tweets?.Meta;
                    tweets?
                    .Where(_ =>
                        (target.HasTweet && !_.IsReply && !_.IsRetweet) ||
                        (target.HasReply && _.IsReply) ||
                        (target.HasRetweet && _.IsRetweet))
                    .Foreach(_ =>
                    {
                        string name(object obj) => string.Format(target.IsLikes ? "{1}.{0}" : "{0}", obj, _.User?.UserName);
                        if (target.HasText)
                        {
                            var text = _.Text.Replace("\r", " ").Replace("\n", " ");
                            Log.Info().Out("saving... [{0}] at {1} '{2}'",
                                _.ID,
                                _.CreatedAt,
                                text.Length > 20 ? (text[0..20] + "...") : text);
                            File.WriteAllText(path(name(_.ID), ".txt"), _.Text);
                        }
                        _.Medias?
                            .Select(m => string.IsNullOrEmpty(m.Url) ? twitter.GetTweetAsync(_.ID).Result.Includes?.Media?.FirstOrDefault() : m)
                            .Where(m => (m.IsPhoto && target.HasPhoto) || (m.IsVideo && target.HasVideo) || (m.IsGif && target.HasGif))
                            .Foreach(m =>
                            {
                                Log.Info().Out("downloading... [{0}]({1}) from '{1}'", _.ID, m.ID, m.Url);
                                var regex = new Regex(@"\?.+$");
                                var filename = path(name(m.ID), Path.GetExtension(regex.Replace(m.Url, "")));
                                client.GetAsync(m.Url).Result.Content.DownloadAsync(filename).Wait();
                            });
                    });
                    if (target.NewestId < meta?.NewestId)
                    {
                        target.NewestId = meta?.NewestId;
                        var filename = path(target.UserName, ".newest.txt");
                        File.WriteAllText(filename, target.NewestId.ToString());
                    }
                    count += meta?.ResultCount ?? 0;
                    if (meta.IsEmpty && retry < settings.Retry)
                    {
                        retry++;
                        continue;
                    }
                    else if (meta.IsEmpty) break;
                    else retry = 0;
                    if (!option.Next(meta)) break;
                }
                while (true);
                Log.Info().Out("saved {0} tweets.", count);
            });
#if DEBUG
            ConsoleEx.Pause();
#endif
        }
    }
}
