using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib
{
    public class Arguments
    {
        public const string DEFAULT_OPTION_CHAR = "/";
        protected readonly List<string> _values = new List<string>();
        protected List<string> _args;
        protected CommandMap _map;
        public string this[int index] => index < _values.Count ? _values[index] : null;
        public bool HasValue => _values.Count > 0;
        public bool HasOption(string key) => _map.GetValue<bool>(key);
        public virtual List<string> Values => _values;
        public string OptionCharacter { get; protected set; }
        [Command(@"?")]
        [Command("help")]
        [Detail("view help.")]
        public bool Help { get; set; }
        [Command]
        [Hidden]
        public bool DebugMode { get; set; }
        protected Arguments(IEnumerable<string> args, string option) 
        {
            OptionCharacter = option;
            Initialize();
            Reset(args);
            if (DebugMode) PrintDebug();
            if (Help) {
                PrintHelp();
                Environment.Exit(0);
            }
        }
        protected virtual void Initialize() => _map = new CommandMap(this);
        public virtual void Reset(IEnumerable<string> args)
        {
            _args = args?.ToList();
            _values.Clear();
            for (var i = 0; i < _args.Count; i++)
            {
                var value = _args[i];
                if (value.Length > 0 && value.Substring(0, 1) == OptionCharacter)
                {
                    value = value.ToLower();
                    if (_map.IsList(value))
                    {
                        var (o, p) = _map[value];
                        var list = p.GetValue(o) as IList ?? Activator.CreateInstance(p.PropertyType) as IList;
                        var a = _args[++i];
                        list.Add(value is string ? Convert.ChangeType(a, p.PropertyType.GetGenericArguments()[0]) : a);
                        _map.SetValue(value, list);
                    }
                    else _map.SetValue(value, _map.WithNextValue(value) ? _args[++i] : "true");
                }
                else _values.Add(value);
            }
        }
        public virtual void Reset() => Reset(Environment.GetCommandLineArgs().Skip(1));
        public virtual void PrintDebug() => Console.WriteLine(this);
        public virtual void PrintHelp() => Console.WriteLine(_map);
        public override string ToString() =>  string.Format("values : {0}, options : {1}", string.Join(",", _values), _map);

        public static Arguments Load() => Load(DEFAULT_OPTION_CHAR);
        public static Arguments Load(string option) => Load(Environment.GetCommandLineArgs().Skip(1), option);
        public static Arguments Load(IEnumerable<string> args) => Load(args, DEFAULT_OPTION_CHAR);
        public static Arguments Load(IEnumerable<string> args, string option) => new Arguments(args, option);
        public static Arguments<T> Load<T>() => Load<T>(DEFAULT_OPTION_CHAR);
        public static Arguments<T> Load<T>(string option) => Load<T>(Environment.GetCommandLineArgs().Skip(1), option);
        public static Arguments<T> Load<T>(IEnumerable<string> args) => Load<T>(args, DEFAULT_OPTION_CHAR);
        public static Arguments<T> Load<T>(IEnumerable<string> args, string option) => new Arguments<T>(args, option);
    }
    public class Arguments<T> : Arguments
    {
        T _options;
        public T Options => _options;
        internal Arguments(IEnumerable<string> args, string option) : base(args, option) { }
        protected override void Initialize()
        {
            base.Initialize();
            _options = (T)typeof(T).GetConstructor(new Type[] { }).Invoke(new object[] { });
            _map.Add(_options);
        }
    }

}
