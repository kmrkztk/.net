using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Lib;

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
            using var client = TwitterClient.V1_1(bearer);
            var user = client.GetUser("Twitter");
            Console.WriteLine(user);
            var tl = client.GetTimeline(user);
            foreach (var t in tl)
            {
                Console.WriteLine(t);
                foreach (var m in t.Medias ?? Enumerable.Empty<Media>()) m.DownloadAsync().Wait();
            }
            /*
            var favorite = client.GetLikes(account["data"]["id"].Value);
            foreach (var a in favorite.AsArray().Select(_=>_["id"])) Console.WriteLine(a);

            foreach (var a in favorite.AsArray().Select(_ => _["id"]))
            {
                var show = client.GetShow(a.Value);
                Console.WriteLine(show);
            }
            //var timeline = client.GetTimeline(account["data"]["id"].Value);
            //foreach (var t in timeline.AsObject().Find("text")) Console.WriteLine(t);
            */
            ConsoleEx.Pause();
        }


    }
}
