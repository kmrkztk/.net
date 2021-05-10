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
using Lib.Json;
using Lib.Web.Twitter;
using Lib.Text;

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
                do
                {
                    var timeline = client.GetTimelineAsync(target.UserName, option).Result;
                    var medias = timeline.Includes.Medias.ToDictionary(_ => _.Key);
                    timeline.Foreach(_ =>
                    {
                        Console.WriteLine("saving... [{0}] {1}", _.ID, _.CreatedAt);
                        if (target.HasText) target.Download(_);
                        _.Attachments?
                            .MediaKeys?
                            .Select(k => medias[k])
                            .Select(m => m.IsVideo ? client.GetTweetAsync(_.ID).Result.Includes.Medias.FirstOrDefault() : m)
                            .Where(m => (m.IsPhoto && target.HasPhoto) || (m.IsVideo && target.HasVideo))
                            .Foreach(m => target.Download(m));
                    });
                    target.Download(timeline.Meta);
                    if (timeline.Meta.ResultCount == 0) break;
                    option.EndTime = timeline.Min(_ => _.CreatedAt)?.AddSeconds(-1);
                }
                while (true);
            });
#if DEBUG
            ConsoleEx.Pause();
#endif
        }
    }
}
