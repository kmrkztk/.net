using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Lib.Reflection;

namespace Lib.Configuration
{
    public delegate void ConfigWachedEventHandler(object sender, FileSystemEventArgs e);
    public sealed class Config
    {
        readonly object _instance;
        readonly Type _type;
        readonly List<Property> _props;
        readonly string _filename;
        readonly ConfigLoader _loader;
        readonly FileSystemWatcher _watcher;
        readonly ConfigWachedEventHandler _changed;
        Config(Type type, string filename, ConfigLoader loader, bool watch, ConfigWachedEventHandler changed)
        {
            _type = type;
            _filename = filename;
            _loader = loader;
            _instance = _loader.Load(_filename, _type);
            _props = Property.GetProperties(_instance).ToList();
            _changed = changed;
            if (watch || changed != null)
            {
                var fi = new FileInfo(_filename);
                _watcher = new FileSystemWatcher()
                {
                    Path = fi.Directory.FullName,
                    Filter = fi.Name,
                    IncludeSubdirectories = false,
                    EnableRaisingEvents = true,
                };
                if (watch) _watcher.Changed += (sender, e) =>
                {
                    if (e.ChangeType != WatcherChangeTypes.Changed) return;
                    Refresh();
                };
                if (changed != null) _watcher.Changed += (sender, e) => _changed?.Invoke(sender, e);
            }
        }
        public void Refresh()
        {
            if (!File.Exists(_filename)) return;
            var instance = _loader.Load(_filename, _type);
            _props.Do(_ => _.SetValue(_.Info.GetValue(instance)));
        }

        readonly static List<Config> _configs = new();
        public static T Load<T>() => Load<T>(typeof(T).GetCustomAttribute<ConfigAttribute>());
        public static T Load<T>(ConfigWachedEventHandler changed) => Load<T>(typeof(T).GetCustomAttribute<ConfigAttribute>(), changed);
        public static T Load<T>(ConfigAttribute attribute) => Load<T>(attribute, null);
        public static T Load<T>(ConfigAttribute attribute, ConfigWachedEventHandler changed) => Load<T>(attribute.FileName, ConfigLoader.GetLoader(attribute.Type), attribute.Watching, changed);
        public static T Load<T>(string filename, ConfigLoader loader, bool watch) => Load<T>(filename, loader, watch, null);
        public static T Load<T>(string filename, ConfigLoader loader, bool watch, ConfigWachedEventHandler changed)
        {
            var config = new Config(typeof(T), filename, loader, watch, changed);
            _configs.Add(config);
            return (T)config._instance;
        }
    }
}