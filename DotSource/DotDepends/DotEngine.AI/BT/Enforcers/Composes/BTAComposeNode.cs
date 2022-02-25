using DotEngine.AI.BT.Datas;
using System.Collections.Generic;

namespace DotEngine.AI.BT.Enforcers
{
    public abstract class BTAComposeNode : BTAExecutorNode
    {
        protected List<BTAExecutorNode> ExecutorNodes = new List<BTAExecutorNode>();

        protected BTComposeNodeData ComposeData { get; private set; }

        public override void DoInitilize(BTController controller, BTNodeData data)
        {
            base.DoInitilize(controller, data);

            ComposeData = data as BTComposeNodeData;
            if(ComposeData!=null && ComposeData.ExecutorDatas!=null)
            {

            }
        }

        public override void DoDestroy()
        {
            ExecutorNodes.Clear();
            ComposeData = null;

            base.DoDestroy();
        }
    }
}
