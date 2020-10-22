using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lib;
using Lib.Windows.Drawing;

namespace Lib.Windows.Gaming
{
    public partial class Animator : Lib.Windows.Controls.Component
    {
        public Animator() : this(null) { }
        public Animator(IContainer container)
        {
            container?.Add(this);
            InitializeComponent();
            Disposed += (sender, e) => OnDisposed();
        }
        Task _task;
        bool _running = false;
        bool _disposed = false;
        public event EventHandler Ran;
        public event EventHandler Stopped;
        public event EventHandler Done;
        [DefaultValue(60)] public virtual float FramePerSec { get; set; } = 60;
        [Browsable(false)] public virtual float FrameRate => 1000 / FramePerSec;
        [Browsable(false)] public virtual float RealFrameRate { get; private set; }
        [Browsable(false)] public virtual float RealFramePerSec => RealFrameRate > 0 ? 1000 / RealFrameRate : -1;
        [Browsable(false)] public virtual ulong Steps { get; private set; }
        [Browsable(false)] public virtual bool IsRunning => _running;
        protected override bool CanRaiseEvents => true;
        [DefaultValue(false)] public bool SkipDraw { get; set; }
        public Drawer Drawer { get; set; }
        public Calculator Calculator { get; set; }
        protected void OnDisposed()
        {
            _running = false;
            _disposed = true;
        }
        protected virtual void OnRan(EventArgs e) => Ran?.Invoke(this, e);
        protected virtual void OnStopped(EventArgs e) => Stopped?.Invoke(this, e);
        protected virtual void OnDone(EventArgs e) => Done?.Invoke(this, e);
        public void Run()
        {
            _task = _task ?? Task.Run(_Run);
            _running = true & !_disposed;
            OnRan(EventArgs.Empty);
        }
        public void Stop()
        {
            _running = false;
            OnStopped(EventArgs.Empty);
        }
        void _Run()
        {
            try
            {
                var t = new Tick();
                while (!_disposed)
                {
                    if (_running)
                    {
                        unchecked { Steps++; }
                        Calculator?.Execute();
                        if (t.Elapse < FrameRate || !SkipDraw) Drawer?.Render();
                    }
                    var elapse = t.Elapse;
                    var sleep = (int)(FrameRate - elapse);
                    RealFrameRate += elapse;
                    RealFrameRate /= 2;
                    OnDone(EventArgs.Empty);
                    if (sleep > 0) Thread.Sleep(sleep);
                    t.Reset();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                _running = false;
                _task = null;
            }
        }
        class Tick
        {
            int _tick;
            public Tick() => Reset();
            public int Elapse => Environment.TickCount - _tick;
            public void Reset() => _tick = Environment.TickCount;
            public override string ToString() => string.Format("{0}ms", Elapse);
        }
    }
}
