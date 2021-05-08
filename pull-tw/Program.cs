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
                    timeline.Foreach(_ =>
                    {
                        Console.WriteLine("saving... [{0}] {1}", _.ID, _.CreatedAt);
                        if (target.HasText) target.Download(_);
                        _.Medias?
                            .Where(_ => _ != null)
                            .Where(_ => (_.IsPhoto && target.HasPhoto) || (_.IsVideo && target.HasVideo))
                            .Foreach(_ => target.Download(_));
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
