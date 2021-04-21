using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RangeAttribute : ValidationAttribute
    { 
        public override string DefaultMessage => "'{name}' is out of range" + 
            Min != null && Max != null ? "({min} ~ {max})." :
            Min != null ? "({min} ~)" :
            Max != null ? "(~ {max})" :
            "";
        public decimal? Min { get; } = null;
        public decimal? Max { get; } = null;
        public override string GetMessage(Property property, ValidationContext context)
            => base.GetMessage(property, context)
            .Replace("{min}", Min.ToString())
            .Replace("{max}", Max.ToString());
        public override bool HasError(Property property, ValidationContext context)
            => decimal.TryParse(property.GetValue()?.ToString(), out var d)
            && d < (Min ?? decimal.MinValue)
            && d > (Max ?? decimal.MaxValue);
    }
}
