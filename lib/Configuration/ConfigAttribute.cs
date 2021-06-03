using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Configuration
{
    [AttributeUsage(
        AttributeTargets.Class | 
        AttributeTargets.Interface | 
        AttributeTargets.Struct)]
    public class ConfigAttribute : Attribute
    {
        public string FileName { get; }
        public ConfigType Type { get; init; } = ConfigType.Json;
        public bool Watching { get; init; }
        public ConfigAttribute(string filename) => FileName = filename;
    }
    public enum ConfigType
    {
        Json,
        Xml,
        Ini,
        Original,
    }
}
