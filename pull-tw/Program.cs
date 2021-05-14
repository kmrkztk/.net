﻿using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using Lib;
using Lib.Web.Twitter;
using Lib.Web.Twitter.Objects;
using Lib.Web.Twitter.Options;
using Lib.Entity;

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
                Console.WriteLine("pull '{0}'...", target.UserName);
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
                        if (target.HasText)
                        {
                            var text = _.Text.Replace("\r", " ").Replace("\n", " ");
                            Console.WriteLine("saving... [{0}] at {1} '{2}'",
                                _.ID,
                                _.CreatedAt,
                                text.Length > 20 ? (text[0..20] + "...") : text);
                            File.WriteAllText(path(_.ID, ".txt"), _.Text);
                        }
                        _.Medias?
                            .Select(m => string.IsNullOrEmpty(m.Url) ? twitter.GetTweetAsync(_.ID).Result.Includes?.Media?.FirstOrDefault() : m)
                            .Where(m => (m.IsPhoto && target.HasPhoto) || (m.IsVideo && target.HasVideo) || (m.IsGif && target.HasGif))
                            .Foreach(m =>
                            {
                                Console.WriteLine("downloading... [{0}] from '{1}'", m.ID, m.Url);
                                var regex = new Regex(@"\?.+$");
                                var filename = path(m.ID, Path.GetExtension(regex.Replace(m.Url, "")));
                                client.DownloadAsync(m.Url, filename).Wait();
                            });
                    });
                    if (target.NewestId < meta?.NewestId)
                    {
                        target.NewestId = meta?.NewestId;
                        var filename = path(target.UserName, ".newest.txt");
                        File.WriteAllText(filename, target.NewestId.ToString());
                    }
                    count += meta?.ResultCount ?? 0;
                    Console.WriteLine("next? {0}", meta);
                    if (meta.IsEmpty && retry < settings.Retry)
                    {
                        retry++;
                        Console.WriteLine("retry");
                        continue;
                    }
                    else if (meta.IsEmpty) break;
                    else retry = 0;
                    if (!option.Next(meta)) break;
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
