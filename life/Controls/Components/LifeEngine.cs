using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace life.Controls
{
    public partial class LifeEngine : Component
    {
        public LifeEngine() : this(null) { }
        public LifeEngine(IContainer container)
        {
            container?.Add(this);
            InitializeComponent();
        }
        protected override void AddHandler(MapView view)
        {
            base.AddHandler(view);
            _animator.Drawer = view.BaseDrawer;
            _calc.Map = view.Map;
        }
        [Browsable(false)] public LifeAnimator Animator => _animator;
        [Browsable(false)] public bool IsRunning => _animator.IsRunning;
        public void Run() => _animator.Run();
        public void Stop() => _animator.Stop();
    }
}
