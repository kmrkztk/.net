using Lib.Windows.Controls.Design;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Windows.Controls
{
    public class ComponentCollection<T> : Collection<T> where T : System.ComponentModel.Component
    {
    }
}
