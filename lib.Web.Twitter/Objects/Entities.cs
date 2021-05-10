using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Web.Twitter.Objects
{
    public class Entities
    {
        public class Annotation
        {
            [LowerName] public int? Start { get; set; }
            [LowerName] public int? End { get; set; }
            [LowerName] public decimal? Probability { get; set; }
            [LowerName] public string Type { get; set; }
            [SnakeCaseName] public string NormalizedText { get; set; }
        }
        public class Url
        {
            [LowerName] public int? Start { get; set; }
            [LowerName] public int? End { get; set; }
            [Name("url")] public string Value { get; set; }
            [LowerName] public int? Status { get; set; }
            [LowerName] public string Title { get; set; }
            [LowerName] public string Description { get; set; }
            [LowerName] public List<Image> Images { get; set; }
            [SnakeCaseName] public string ExpandedUrl { get; set; }
            [SnakeCaseName] public string DisplayUrl { get; set; }
            [SnakeCaseName] public string UnwoundUrl { get; set; }
        }
        public class HashTag
        {
            [LowerName] public int? Start { get; set; }
            [LowerName] public int? End { get; set; }
            [LowerName] public string Tag { get; set; }
        }
        public class Mention
        {
            [LowerName] public int? Start { get; set; }
            [LowerName] public int? End { get; set; }
            [LowerName] public string UserName { get; set; }
        }
        public class CashTag
        {
            [LowerName] public int? Start { get; set; }
            [LowerName] public int? End { get; set; }
            [LowerName] public string Tag { get; set; }
        }
        public class Image
        {
            [LowerName] public string Url { get; set; }
            [LowerName] public int Height { get; set; }
            [LowerName] public int Width { get; set; }
        }
        [LowerName] public List<Annotation> Annotations { get; set; }
        [LowerName] public List<Url> Urls { get; set; }
        [LowerName] public List<HashTag> HashTags { get; set; }
        [LowerName] public List<Mention> Mentions { get; set; }
        [LowerName] public List<CashTag> CashTags { get; set; }
    }
}
