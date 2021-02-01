using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.BL.Executor.Action
{
    public abstract class AEventActionExecutor : AActionExecutor
    {
        public abstract void DoTrigger();
    }
}
