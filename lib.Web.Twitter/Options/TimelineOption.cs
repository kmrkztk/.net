using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;
using Lib.Entity;
using Lib.Web.Twitter.Objects;

namespace Lib.Web.Twitter.Options
{
    public class TimelineOption : Option, INextOption
    {
        [SnakeCaseName] [Format("yyyy-MM-ddTHH:mm:ssZ")] public DateTime? StartTime { get; set; }
        [SnakeCaseName] [Format("yyyy-MM-ddTHH:mm:ssZ")] public DateTime? EndTime { get; set; }
        [SnakeCaseName]                      public ID? SinceId { get; set; }
        [SnakeCaseName]                      public ID? UntilId { get; set; }
        [SnakeCaseName]                      public int? MaxResults { get; set; }
        [SnakeCaseName]                      public string PaginationToken { get; set; }
        [SnakeCaseName] [FlagsFormat]        public ExcludeOptions Exclude { get; set; }
        [SnakeCaseName] [FlagsFormat]        public ExpansionsOptions Expansions { get; set; }
        [Name("tweet.fields")] [FlagsFormat] public TweetFieldsOptions TweetFields { get; set; }
        [Name("media.fields")] [FlagsFormat] public MediaFieldsOptions MediaFields { get; set; }
        [Name("place.fields")] [FlagsFormat] public PlaceFieldsOptions PlaceFields { get; set; }
        [Name("poll.fields")]  [FlagsFormat] public PollFieldsOptions PollFields { get; set; }
        [Name("user.fields")]  [FlagsFormat] public UserFieldsOptions UserFields { get; set; }


        public bool Next(Meta meta)
        {
            UntilId = meta.OldestId - 1;
            return meta.ResultCount > 0;
        }
    }
}
