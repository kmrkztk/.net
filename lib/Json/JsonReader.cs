using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Json
{
    public class JsonReader : TextReader
    {
        readonly TextReader _reader;
        bool _in;
        bool _esc;
        int _row;
        int _col;
        char _c;
        char _prev;
        readonly Queue<char> _last = new();
        public TextReader Base => _reader;
        public bool IsInString => _in;
        public bool Escape => _esc;
        public int Row => _row;
        public int Column => _col;
        public JsonReader(TextReader reader) => _reader = reader;
        public bool EndOfText => Peek() < 0;
        public override int Peek()
        {
            int c;
            while ((c = _reader.Peek()) >= 0)
            {
                if (!IsAvail((char)c))
                {
                    Next();
                    continue;
                }
                break;
            }
            return c;
        }
        public override int Read()
        {
            var c = Peek();
            Next();
            return c;
        }
        public string ReadBlock()
        {
            var s = new StringBuilder();
            int c;
            void next()
            {
                s.Append((char)c);
                Next();
            }
            while ((c = Peek()) >= 0)
            {
                var c_ = (char)c;
                if (_in)
                {
                    switch (c_)
                    {
                        case Json.BlockChar:
                            if (_c == Json.EscapeChar) break;
                            next();
                            return s.ToString();
                    }
                }
                else
                {
                    switch (c_)
                    {
                        case Json.ObjectChar:
                        case Json.ArrayChar:
                            if (s.Length > 1) throw FormatException();
                            next();
                            return s.ToString();
                        case Json.ObjectCharEnd:
                        case Json.ArrayCharEnd:
                        case Json.Separator:
                        case Json.ValueSeparator:
                            if (s.Length >= 1) return s.ToString();
                            next();
                            return s.ToString();
                        case Json.BlockChar:
                            if (_c == Json.EscapeChar) break;
                            if (s.Length > 1) throw FormatException();
                            break;
                    }
                }
                next();
            }
            return s.ToString();
        }
        
        void Next()
        {
            _prev = _c;
            _c = (char)_reader.Read();
            _last.Enqueue(_c);
            if (_last.Count > 20) _last.Dequeue();
            _col++;
            if ((_c == Json.LF && _prev != Json.CR)
            || _c == Json.CR)
            {
                _row++;
                _col = 0;
            }
            if (_in)
            {
                if (_esc) _esc = false;
                else if (_c == Json.BlockChar) _in = false;
                else if (_c == Json.EscapeChar) _esc = true;
            }
            else
            {
                if (_c == Json.BlockChar) _in = true;
            }
        }
        bool IsAvail(char c)
        {
            if (_in)
            {
                if (_esc) return true;
                /*
                if (c == Json.LF) throw FormatException();
                if (c == Json.CR) throw FormatException();
                if (c == Json.Tab) throw FormatException();
                */
            }
            else
            {
                switch (c)
                {
                    case Json.LF: case Json.CR: case Json.Tab: case (char)0x0020: case (char)0x00A0: return false;
                    case Json.EscapeChar: throw FormatException();
                }
            }
            return true;
        }
        public JsonFormatException FormatException() => new(_row, _col, new string(_last.ToArray()));
        public JsonFormatException FormatException(string message) => new(_row, _col, new string(_last.ToArray()), message);
    }
}
