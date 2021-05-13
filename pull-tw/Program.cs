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
using Lib.Jsons;
using Lib.Web.Twitter;
using Lib.Text;
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
                do
                {
                    var timeline = client.GetTimelineAsync(target.UserName, option).Result;
                    var medias = timeline.Includes?.Medias?.ToDictionary(_ => _.Key) ?? new();
                    timeline
                    .Where(_ =>
                        (target.HasTweet && !_.IsReply && !_.IsRetweet) ||
                        (target.HasReply && _.IsReply) ||
                        (target.HasRetweet && _.IsRetweet))
                    .Foreach(_ =>
                    {
                        Console.WriteLine("saving... [{0}] {1}", _.ID, _.CreatedAt);
                        if (target.HasText) target.Download(_);
                        _.Attachments?
                            .MediaKeys?
                            .Where(k => medias.ContainsKey(k))
                            .Select(k => medias[k])
                            .Select(m => m.IsVideo ? client.GetTweetAsync(_.ID).Result.Includes.Medias.FirstOrDefault() : m)
                            .Where(m => (m.IsPhoto && target.HasPhoto) || (m.IsVideo && target.HasVideo))
                            .Foreach(m => target.Download(m));
                    });
                    target.Download(timeline.Meta);
                    Console.WriteLine(timeline.Meta);
                    if (timeline.Meta?.OldestId != ID.Null) 
                        old = old < timeline.Meta.OldestId ? old : timeline.Meta.OldestId;
                    count += timeline.Meta?.ResultCount ?? 0;
                    option.NextToken = timeline.Meta?.NextToken;
                    if (string.IsNullOrEmpty(option.NextToken))
                    {
                        if (old == ID.Max) break;
                        if (option.UntilId == old) break;
                        option.UntilId = old;
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
