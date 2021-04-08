using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Lib.Collections;
using Lib.Reflection;

namespace Lib
{
    /// <summary>
    /// class Arguments.
    /// 
    /// 
    /// </summary>
    public class Arguments : IEnumerable<string>
    {
        protected readonly List<string> _values = new List<string>();
        protected List<string> _args;
        protected PropertyMap _map;
        public string this[int index] => index < _values.Count ? _values[index] : null;
        public int Count => _values.Count;
        public bool HasValue => _values.Count > 0;
        [Command(@"?")]
        [Command("help")]
        [Detail("view help.")]
        public bool Help { get; set; }
        [Command]
        [Hidden]
        public bool DebugMode { get; set; }
        protected Arguments(IEnumerable<string> args) 
        {
            Initialize();
            Reset(args);

            PrintDebug(this);
            if (Help) GoHelp();
        }
        protected virtual void Initialize() => _map = PropertyMap.Of(this);
        public virtual void Reset(IEnumerable<string> args)
        {
            _args = args?.ToList();
            _values.Clear();
            for (var i = 0; i < _args.Count; i++)
            {
                var value = _args[i];
                var key = value.ToLower();
                if (_map.ContainsKey(key)) foreach (var p in _map[key]) p.AddValue(p.Info.HasAttribute<CommandValueAttribute>() ? _args[++i] : "true");
                else _values.Add(value);
            }
        }
        public virtual void Reset() => Reset(Environment.GetCommandLineArgs().Skip(1));
        public void PrintDebug(object value) { if (DebugMode) Console.WriteLine(value); }
        public void PrintDebug(string format, params object[] arg) { if (DebugMode) Console.WriteLine(format, arg); }
        public void PrintHelp() => Console.WriteLine(ToHelp());
        public void GoHelp()
        {
            PrintHelp();
            Environment.Exit(0);
        }
        public virtual IEnumerator<string> GetEnumerator() => _values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public override string ToString() => string.Join(" ", _args);
        public string ToHelp()
        {
            var s = new StringBuilder();
            s.AppendLine(Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName));
            s.AppendLine();
            s.AppendLine("[help]");
            foreach (var o in _map
                .Select(_ => _.Value.Instance)
                .Distinct()
                .SelectMany(_ => DetailAttribute.GetContents(_))
                .Select(_ => "  " + _)) s.AppendLine(o);
            s.AppendLine();
            s.AppendLine("[option]");
            var map = _map
                .GroupBy(_ => _.Value.Info)
                .Where(_ => !_.Key.HasAttribute<HiddenAttribute>())
                .ToDictionary(_ => _.Key, _ => string.Join(" or ", _.Select(i => i.Key)));
            var padding = map.Values.Max(_ => _.Length);
            foreach (var o in map
                .Select(_ => string.Format("  {0} : {1}",
                _.Value.PadRight(padding), string.Concat(DetailAttribute.GetContents(_.Key))))) s.AppendLine(o);
            return s.ToString();
        }
        public static Arguments Load() => Load(Environment.GetCommandLineArgs().Skip(1));
        public static Arguments Load(IEnumerable<string> args) => new Arguments(args);
    }

    public class Arguments<T> : Arguments
    {
        T _options;
        public T Options => _options;
        internal Arguments(IEnumerable<string> args) : base(args) { }
        protected override void Initialize()
        {
            base.Initialize();
            _options = (T)typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { });
            _map.Add(PropertyMap.Of(_options));
        }
        public static new Arguments<T> Load() => Load(Environment.GetCommandLineArgs().Skip(1));
        public static new Arguments<T> Load(IEnumerable<string> args) => new Arguments<T>(args);
    }
}
