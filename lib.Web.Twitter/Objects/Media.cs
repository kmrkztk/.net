using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Entity;
using Lib.Jsons;

namespace Lib.Web.Twitter.Objects
{
    public class Media
    {
        public const string Photo = "photo";
        public const string Video = "video";
        public const string Gif = "animated_gif";
        public bool IsPhoto => Type == Photo;
        public bool IsVideo => Type == Video;
        public bool IsGif => Type == Gif;
        string _key;
        [Name("id")] public ID ID { get; set; }
        [Name("media_key")] public string Key 
        {
            get => _key;
            set
            {
                _key = value;
                ID = _key?.Split("_")[1];
            }
        }
        [SnakeCaseName] public string Type { get; set; }
        [SnakeCaseName] public string Url { get; set; }
        [SnakeCaseName] public int DurationMs { get; set; }
        [SnakeCaseName] public int Height { get; set; }
        [SnakeCaseName] public int Width { get; set; }
        [SnakeCaseName] public string PreviewImageUrl { get; set; }
        [SnakeCaseName] public Metrics PublicMetrics { get; set; }
        [SnakeCaseName] public Metrics NonPublicMetrics { get; set; }
        [SnakeCaseName] public Metrics OrganicMetrics { get; set; }
        [SnakeCaseName] public Metrics PromotedMetrics { get; set; }
        public override string ToString() => string.Format("({0})[{1}]{2}", ID, Type, Url);
        public class Metrics
        {
            [SnakeCaseName] public int ViewCount { get; set; }
            [SnakeCaseName] public int Playback0Count { get; set; }
            [SnakeCaseName] public int Playback25Count { get; set; }
            [SnakeCaseName] public int Playback50Count { get; set; }
            [SnakeCaseName] public int Playback75Count { get; set; }
            [SnakeCaseName] public int Playback100Count { get; set; }
        }
        public static Media OfVer1(Json json) => json == null ? null : new()
        {
            ID = json["id"].Value,
            Type = json["type"].Value,
            Url =
                json["type"].Value == "photo" ? json["media_url"].Value :
                json["type"].Value == "video" ||
                json["type"].Value == "animated_gif" ? json["video_info"]["variants"]
                    .AsArray()
                    .OrderBy(_ => _["bitrate"]?.Value ?? "a")
                    .FirstOrDefault()?["url"].Value :
                null,
        };
    }
}
