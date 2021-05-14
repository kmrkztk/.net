using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;
using Lib.Entity;

namespace Lib.Web.Twitter.Options
{
    public class TweetOption : Option
    {
        [LowerName] public ID ID { get; set; }
        [SnakeCaseName] [LowerFormat] public bool? TrimUser { get; set; } = true;
        [SnakeCaseName] [LowerFormat] public bool? IncludeMyRetweet { get; set; } = false;
        [SnakeCaseName] [LowerFormat] public bool? IncludeEntities { get; set; } = true;
        [SnakeCaseName] [LowerFormat] public bool? IncludeExtAltText { get; set; } = false;
        [SnakeCaseName] [LowerFormat] public bool? IncludeCardUri { get; set; } = false;
        [SnakeCaseName] [FlagsFormat] public TweetModes TweetMode { get; set; } = TweetModes.Extended;
        public TweetOption(ID id) => ID = id;
    }
}
