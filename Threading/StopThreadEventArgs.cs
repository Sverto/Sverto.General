using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sverto.General.Threading
{
    public class StopThreadEventArgs : EventArgs
    {
        public bool Forced { get; set; }
    }
}
