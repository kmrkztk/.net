using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Windows.Controls.Status
{
    public interface IStatus
    {
        event EventHandler Updated;
    }
}
