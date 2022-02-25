using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.AI.BT.Enforcers
{
    public class BTParallelComposeNode : BTAComposeNode
    {
        public override EBTResult DoExecute(float deltaTime)
        {
            if(ExecutorNodes.Count == 0)
            {
                return EBTResult.Faiture;
            }
            return EBTResult.Success;
        }
    }
}
