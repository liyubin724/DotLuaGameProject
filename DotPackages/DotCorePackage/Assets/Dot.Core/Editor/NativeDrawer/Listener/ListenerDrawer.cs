using DotEngine.NativeDrawer;
using DotEngine.NativeDrawer.Listener;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEditor.NativeDrawer.Listener
{
    public abstract class ListenerDrawer : AttrNativeDrawer
    {
        public object Target { get; private set; }

        public ListenerDrawer(object target,ListenerAttribute attr) : base(attr)
        {
            Target = target;
        }

        public abstract void Execute();
    }
}
