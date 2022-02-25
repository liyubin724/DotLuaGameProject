using DotEngine.AI.BT.Datas;

namespace DotEngine.AI.BT.Enforcers
{
    public abstract class BTAConditionNode : BTANode
    {
        protected BTConditionNodeData ConditionData { get; private set; }

        public override void DoInitilize(BTController controller, BTNodeData data)
        {
            base.DoInitilize(controller, data);

            ConditionData = data as BTConditionNodeData;
        }

        public abstract bool IsMeet();
    }
}
