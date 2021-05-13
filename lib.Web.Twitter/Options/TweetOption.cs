using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Web.Twitter.Options
{
    public class TweetOption : Option
    {
        [SnakeCaseName] [LowerFormat] public bool? TrimUser { get; set; } = true;
        [SnakeCaseName] [LowerFormat] public bool? IncludeMyRetweet { get; set; } = false;
        [SnakeCaseName] [LowerFormat] public bool? IncludeEntities { get; set; } = true;
        [SnakeCaseName] [FlagsFormat] public TweetModes TweetMode { get; set; } = TweetModes.Extended;
    }
}
