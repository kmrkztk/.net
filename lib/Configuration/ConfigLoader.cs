using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Lib.Reflection;
using Lib.Jsons;

namespace Lib.Configuration
{
    public abstract class ConfigLoader
    {
        public object Load(string filename, Type type)
        {
            using var fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return Load(fs, type);
        }
        public abstract object Load(Stream stream, Type type);
        public class JsonConfigLoader : ConfigLoader { public override object Load(Stream stream, Type type) => Json.Load(stream).Cast(type); }
        public class XmlConfigLoader : ConfigLoader { public override object Load(Stream stream, Type type) => new XmlSerializer(type).Deserialize(stream); }
        public class IniConfigLoader : ConfigLoader { public override object Load(Stream stream, Type type) => new Ini(stream).Cast(type); }
        public static ConfigLoader GetLoader(ConfigType type) =>
            type == ConfigType.Json ? new JsonConfigLoader() :
            type == ConfigType.Xml ? new XmlConfigLoader() :
            type == ConfigType.Ini ? new IniConfigLoader() : null;
    }
}
