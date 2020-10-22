using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Windows.Gaming
{
    public abstract partial class Calculator : Lib.Windows.Controls.Component
    {
        public event EventHandler Calculating;
        public event EventHandler Calculated;
        public Calculator() : this(null) { }
        public Calculator(IContainer container)
        {
            container?.Add(this);
            InitializeComponent();
        }
        public void Execute()
        {
            OnCalculating(EventArgs.Empty);
            Calc();
            OnCalculated(EventArgs.Empty);
        }
        protected virtual void OnCalculating(EventArgs e) => Calculating?.Invoke(this, e);
        protected virtual void OnCalculated(EventArgs e) => Calculated?.Invoke(this, e);
        protected abstract void Calc();
    }
}
