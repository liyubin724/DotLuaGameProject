using DotEngine.AI.BT.Datas;

namespace DotEngine.AI.BT.Enforcers
{
    public class BTLogicalConditionNode : BTAConditionNode
    {
        private BTAConditionNode leftConditionNode;
        private BTAConditionNode rightConditionNode;

        private EBTLogicalOperatorType operatorType;

        public override void DoInitilize(BTController controller, BTNodeData data)
        {
            base.DoInitilize(controller, data);

            BTLogicalConditionData logicalConditionData = ConditionData as BTLogicalConditionData;
            operatorType = logicalConditionData.OperatorType;
        }

        public override bool IsMeet()
        {
            if (ConditionData == null)
            {
                return false;
            }

            if (leftConditionNode != null && rightConditionNode != null)
            {
                if (operatorType == EBTLogicalOperatorType.And)
                {
                    return leftConditionNode.IsMeet() && rightConditionNode.IsMeet();
                }
                else if (operatorType == EBTLogicalOperatorType.Or)
                {
                    return leftConditionNode.IsMeet() || rightConditionNode.IsMeet();
                }
                else
                {
                    return false;
                }
            }
            else if (leftConditionNode != null)
            {
                return leftConditionNode.IsMeet();
            }
            else if (rightConditionNode != null)
            {
                return rightConditionNode.IsMeet();
            }
            else
            {
                return false;
            }
        }

        public override void DoDestroy()
        {
            leftConditionNode = null;
            rightConditionNode = null;
            base.DoDestroy();
        }
    }
}
