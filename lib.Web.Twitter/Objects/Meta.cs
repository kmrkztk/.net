using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Entity;

namespace Lib.Web.Twitter.Objects
{
    public class Meta
    {
        [SnakeCaseName] public int ResultCount { get; init; }
        [SnakeCaseName] public ID? OldestId { get; init; }
        [SnakeCaseName] public ID? NewestId { get; init; }
        [SnakeCaseName] public string NextToken { get; init; }
        public bool IsEmpty => ResultCount == 0 && OldestId == null && NewestId == null && NextToken == null;
        public override string ToString() => IsEmpty ? "meta is empty" : string.Format("{0}s ({1}~{2}) '{3}'", ResultCount, NewestId, OldestId, NextToken);
    }
}
