using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib;
using Lib.Entity;
using Lib.Json;

namespace Lib.Web.Twitter.Objects
{
    public class User
    {
        [LowerName] public ID Id { get; set; }
        [LowerName] public string Name { get; set; }
        [LowerName] public string UserName { get; set; }
        [SnakeCaseName] public DateTime? CreatedAt { get; set; }
        [SnakeCaseName] public string Description { get; set; }
        [SnakeCaseName] public Entities Entities { get; set; }
        [SnakeCaseName] public string Location { get; set; }
        [SnakeCaseName] public ID PinnedTweetId { get; set; }
        [SnakeCaseName] public string ProfileImageUrl { get; set; }
        [SnakeCaseName] public bool Protected { get; set; }
        [SnakeCaseName] public Metrics PublicMetrics { get; set; }
        [SnakeCaseName] public string Url { get; set; }
        [SnakeCaseName] public bool Verified { get; set; }
        [SnakeCaseName] public Withheld Withheld { get; set; }

        public override string ToString() => string.Format("({0}){1}", Id, Name);
        public User() { }
        public User(JsonObject json)
        {
            Id = json["id"]?.Value;
            Name = json["name"]?.Value;
            UserName = json["username"]?.Value;

        }
        public class Metrics
        {
            [SnakeCaseName] public int FollowersCount { get; set; }
            [SnakeCaseName] public int FollowingCount { get; set; }
            [SnakeCaseName] public int TweetCount { get; set; }
            [SnakeCaseName] public int ListedCount { get; set; }
        }
    }
}
