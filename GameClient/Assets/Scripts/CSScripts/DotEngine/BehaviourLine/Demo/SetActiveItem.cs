using DotEngine.BehaviourLine.Action;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.BehaviourLine.Demo
{
    [ActionItemBindData(typeof(SetActiveData))]
    public class SetActiveItem : EventActionItem
    {
        public SetActiveItem() : base()
        {
        }

        public override void DoTrigger()
        {
            throw new NotImplementedException();
        }
    }
}
