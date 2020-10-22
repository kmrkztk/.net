using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lib;
using Lib.Text;

namespace base64
{
    class Program
    {
        static void Main()
        {
            ConsoleEx.LoggingUnhandledException();
            var a = FileArguments.Load<Options>();
            if (a.Help) return;
            if (a.Options.Decode)
            {
                using (var o = a.Options.GetOutputStream())
                {
                    foreach (var r in a.GetReaders()) foreach(var r_ in SplitForDecode(r)) Base64Encoder.Decode(r_, o);
                }
            }
            else
            {
                using (var o = a.Options.GetOutputWriter())
                {
                    if (a.Options.Binary)
                    {
                        foreach (var s in a.GetStreams()) o.Write(Base64Encoder.Encode(s).SplitLine(a.Options.Split));
                    }
                    else
                    {
                        foreach (var r in a.GetReaders()) o.Write(Base64Encoder.Encode(r).SplitLine(a.Options.Split));
                    }
                }
            }
        }
        static IEnumerable<TextReader> SplitForDecode(TextReader reader) 
        {
            int c;
            var pad = false;
            var sb = new StringBuilder();
            while ((c = reader.Read()) >= 0)
            {
                if (pad && (char)c != '=')
                {
                    yield return new StringReader(sb.ToString());
                    sb.Clear();
                    pad = false;
                }
                sb.Append((char)c);
                if (!pad && (char)c == '=') pad = true;
            }
            yield return new StringReader(sb.ToString());
        }
        [Detail("encode by base64.")]
        class Options 
        {
            [Command]
            [Command("d")]
            [Detail("decode.")]
            public bool Decode { get; set; } = false;
            [Command]
            [Command("o")]
            [CommandValue]
            [Detail("output to (filepath).")]
            public string Output { get; set; } = null;
            [Command]
            [CommandValue]
            [Detail("split encode string by (n) chars.")]
            public int Split { get; set; } = 64;
            [Command]
            [Command("b")]
            [Detail("open file as binary.")]
            public bool Binary { get; set; }
            [Command]
            [Detail("eqaul option /s 0.")]
            public bool Raw { get => Split <= 0; set => Split = -1; }

            public TextWriter GetOutputWriter()
            {
                if (Output == null) return Console.Out;
                return new StreamWriter(new FileStream(Output, FileMode.Create));
            }
            public Stream GetOutputStream()
            {
                if (Output == null) return new ConsoleWriter();
                return new FileStream(Output, FileMode.Create);
            }
            class ConsoleWriter : Stream
            {
                readonly TextWriter _writer = Console.Out;
                public override bool CanRead => throw new NotImplementedException();
                public override bool CanSeek => throw new NotImplementedException();
                public override bool CanWrite => true;
                public override long Length => throw new NotImplementedException();
                public override long Position { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
                public override void Flush() => _writer.Flush();
                public override int Read(byte[] buffer, int offset, int count) => throw new NotImplementedException();
                public override long Seek(long offset, SeekOrigin origin) => throw new NotImplementedException();
                public override void SetLength(long value) => throw new NotImplementedException();
                public override void Write(byte[] buffer, int offset, int count) => _writer.Write(_writer.Encoding.GetString(buffer, offset, count));
            }
        }
    }
}
