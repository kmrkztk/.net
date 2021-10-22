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
        public ConfigType Type { get; init; } = ConfigType.Original;
        public bool Watching { get; init; } = false;
        public ConfigAttribute(string filename) => FileName = filename;
    }
    [AttributeUsage(
        AttributeTargets.Class |
        AttributeTargets.Interface |
        AttributeTargets.Struct)]
    public class JsonConfigAttribute : ConfigAttribute
    {
        public JsonConfigAttribute(string filename) : base(filename) => Type = ConfigType.Json;
    }
    [AttributeUsage(
        AttributeTargets.Class |
        AttributeTargets.Interface |
        AttributeTargets.Struct)]
    public class XmlConfigAttribute : ConfigAttribute
    {
        public XmlConfigAttribute(string filename) : base(filename) => Type = ConfigType.Xml;
    }
    [AttributeUsage(
        AttributeTargets.Class |
        AttributeTargets.Interface |
        AttributeTargets.Struct)]
    public class IniConfigAttribute : ConfigAttribute
    {
        public IniConfigAttribute(string filename) : base(filename) => Type = ConfigType.Ini;
    }
    public enum ConfigType
    {
        Json,
        Xml,
        Ini,
        Original,
    }
}
