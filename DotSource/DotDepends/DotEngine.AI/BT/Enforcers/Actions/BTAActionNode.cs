using DotEngine.AI.BT.Datas;

namespace DotEngine.AI.BT.Enforcers
{
    public abstract class BTAActionNode : BTAExecutorNode
    {
        protected BTActionNodeData ActionData { get; private set; }

        public override void DoInitilize(BTController controller, BTNodeData data)
        {
            base.DoInitilize(controller, data);
            ActionData = data as BTActionNodeData;
        }

        public override void DoDestroy()
        {
            ActionData = null;
            base.DoDestroy();
        }
    }
}
