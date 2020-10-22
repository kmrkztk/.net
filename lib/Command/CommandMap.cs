using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Lib
{
    public class CommandMap : PropertyMap
    {
        string _option;
        public CommandMap(Arguments obj)
        {
            _option = obj.OptionCharacter;
            Map(obj);
        }
        public bool WithNextValue(string key) => this[key].Property.GetCustomAttributes<CommandValueAttribute>().Count() > 0;
        protected override string GetMappingName(MappingAttribute a, PropertyInfo p) => _option + base.GetMappingName(a, p).ToLower();
    }
}
