using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RangeValidationAttribute : ValidationAttribute
    { 
        public override string DefaultMessage => "'{name}' is out of range" + (
            Min != double.MinValue && Max != double.MaxValue ? "({min} ~ {max})." :
            Min != double.MinValue ? "({min} ~)" :
            Max != double.MaxValue ? "(~ {max})" :
            "");
        public double Min { get; init; } = double.MinValue;
        public double Max { get; init; } = double.MaxValue;
        public override string GetMessage(Property property, ValidationContext context)
            => base.GetMessage(property, context)
            .Replace("{min}", Min.ToString())
            .Replace("{max}", Max.ToString());
        public override bool HasError(Property property, ValidationContext context) => Compare(Parse(property.GetValue()));
        bool Compare(object value) => Min.CompareTo(value) > 0 || Max.CompareTo(value) < 0;
        static double Parse(object value) => value is string s ? double.Parse(s) : Convert.ToDouble(value);
    }
}
