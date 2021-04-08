using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib;

namespace ls
{
    class Program
    {
        static void Main()
        {
            var a = FileArguments<Options>.Load();
            foreach (var d in a.GetFileSystems()
                .Where(_ => a.Options.DisplayDirectory || (_.Attributes & FileAttributes.Directory) == 0)
                .Select(_ => new Display(a.Options, _))) Console.WriteLine(d);
        }

        class Options
        {
            [Command("f")]    public bool FullPath    { get; set; } = true;
            [Command("t")]    public bool Tree        { get; set; }
            [Command("z")]    public bool DisplaySize { get; set; }
            [Command("time")] public bool DisplayTime { get; set; }
            [Command("d")]    public bool DisplayDirectory { get; set; }
        }
        class Display
        {
            readonly Options _o;
            readonly FileSystemInfo _fi;
            public Display(Options o, FileSystemInfo fi)
            {
                _o = o;
                _fi = fi;
            }
            public override string ToString()
            {
                if (_fi is FileInfo fi)
                {
                    return string.Format("{0}"
                        + (_o.DisplayTime ? " \t{1}" : "")
                        + (_o.DisplaySize ? " \t{2}bytes" : "")
                        , _o.FullPath ? fi.FullName : fi.Name
                        , fi.LastWriteTime
                        , fi.Length
                    );
                }
                else
                {
                    return string.Format("[{0}]"
                        + (_o.DisplayTime ? " \t{1}" : "")
                        , _o.FullPath ? _fi.FullName : _fi.Name
                        , _fi.LastWriteTime
                    );
                }
            }
        }
    }
}
