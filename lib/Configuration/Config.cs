using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib.Reflection;

namespace Lib.Configuration
{
    public sealed class Config
    {
        readonly object _instance;
        readonly Type _type;
        readonly List<Property> _props;
        readonly string _filename;
        readonly ConfigLoader _loader;
        readonly FileSystemWatcher _watcher;
        Config(Type type, string filename, ConfigLoader loader, bool autoRefresh)
        {
            _type = type;
            _filename = filename;
            _loader = loader;
            _instance = _loader.Load(_filename, _type);
            _props = Property.GetProperties(_instance).ToList();
            if (autoRefresh)
            {
                _watcher = new FileSystemWatcher(filename);
                _watcher.Changed += (sender, e) => Refresh();
            }
        }
        public void Refresh()
        {
            if (!File.Exists(_filename)) return;
            var instance = _loader.Load(_filename, _type);
            _props.Foreach(_ => _.SetValue(_.Info.GetValue(instance)));
        }

        readonly static List<Config> _configs = new();
        public static T Load<T>() => Load<T>((ConfigAttribute)typeof(T).GetCustomAttributes(typeof(ConfigAttribute), false).FirstOrDefault() ?? throw new ArgumentNullException(nameof(T)));
        public static T Load<T>(ConfigAttribute attribute) => Load<T>(attribute.FileName, ConfigLoader.GetLoader(attribute.Type), attribute.AutoRefresh);
        public static T Load<T>(string filename, ConfigLoader loader, bool autoRefresh)
        {
            var config = new Config(typeof(T), filename, loader, autoRefresh);
            _configs.Add(config);
            return (T)config._instance;
        }
    }
}