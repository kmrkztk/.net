using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Lib.Reflection;

namespace Lib.Validation
{
    [AttributeUsage(AttributeTargets.Property)]
    public class EarlierDateValidationAttribute : ValidationAttribute
    {
        public override string DefaultMessage => "'{name}' must be a date before '{name2}'.";
        public bool Equal { get; init; }
        public string Name { get; init; }
        public string Format { get; init; }
        public EarlierDateValidationAttribute(string name) => Name = name;
        public override string GetMessage(Property property, ValidationContext context)
            => base.GetMessage(property, context)
            .Replace("{name2}", Name);
        public override bool HasError(Property property, ValidationContext context)
            => Compare(Parse(property.GetValue()), Parse(context.GetPropertyValue(Name)));
        bool Compare(DateTime d1, DateTime d2) => Equal ? d1 > d2 : d1 >= d2;
        DateTime Parse(object value) => 
            value is string s ? 
            Format == null ? 
            DateTime.Parse(s) : 
            DateTime.ParseExact(s, Format, CultureInfo.InvariantCulture) :
            (DateTime)value;
    }
}
