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
                .Foreach(_ =>
            {
                _.CreateSavingTo();
                var option = _.Option;
                do
                {
                    var timeline = client.GetTimelineAsync(_.UserName, option).Result;
                    _.Download(timeline);
                    option.UntilId = timeline.Meta.OldestId - 1;
                    if (timeline.Meta.ResultCount == 0) break;
                }
                while (true);
            });
#if DEBUG
            ConsoleEx.Pause();
#endif
        }
    }
}
