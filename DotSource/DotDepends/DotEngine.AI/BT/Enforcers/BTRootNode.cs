using DotEngine.AI.BT.Attributes;
using DotEngine.AI.BT.Datas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.AI.BT.Enforcers
{
    [BTNodeLinkedData(typeof(BTRootNodeData))]
    public class BTRootNode : BTAExecutorNode
    {
        public override EBTResult DoExecute(float deltaTime)
        {
            throw new NotImplementedException();
        }
    }
}
