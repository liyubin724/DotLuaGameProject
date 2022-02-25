using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.AI.BT.Enforcers
{
    public class BTNotDecoratorNode : BTADecoratorNode
    {
        public override EBTResult DoExecute(float deltaTime)
        {
            if(ExecutorNode == null)
            {

            }
            return EBTResult.Success;
        }
    }
}
