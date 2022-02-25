using DotEngine.AI.BT.Datas;

namespace DotEngine.AI.BT.Enforcers
{
    public class BTNotConditionNode : BTAConditionNode
    {
        private BTAConditionNode conditionNode;

        public override void DoInitilize(BTController controller, BTNodeData data)
        {
            base.DoInitilize(controller, data);
        }

        public override bool IsMeet()
        {
            if(ConditionData == null)
            {
                return false;
            }

            return conditionNode.IsMeet();
        }

        public override void DoDestroy()
        {
            conditionNode = null;

            base.DoDestroy();
        }
    }
}
