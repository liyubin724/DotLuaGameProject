using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.AI.BT
{
    public abstract class ABTActionNode : ABTExecutorNode
    {
        public override bool IsValid()
        {
            return true;
        }

        public override void DoEnter()
        {
        }

        public override void DoExit()
        {
        }
    }
}
