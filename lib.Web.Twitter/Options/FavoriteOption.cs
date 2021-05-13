using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Entity;
using Lib.Reflection;

namespace Lib.Web.Twitter.Options
{
    public class FavoritesOption : Option
    {
        [SnakeCaseName] public string UserId { get; set; }
        [SnakeCaseName] public string ScreenName { get; set; }
        [SnakeCaseName] public int? Count { get; set; }
        [SnakeCaseName] public ID? SinceId { get; set; }
        [SnakeCaseName] public ID? MaxId { get; set; }
        [SnakeCaseName] public bool? IncludeEntities { get; set; } = true;
        [SnakeCaseName] [LowerFormat] public TweetModes TweetMode { get; set; } = TweetModes.Extended;
    }
}
