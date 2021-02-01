using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.BL.Executor.Condition
{
    public abstract class AConditionExecutor : ANodeExecutor
    {
        public abstract bool IsMet();
    }
}
