using DotEngine.AI.BT.Datas;

namespace DotEngine.AI.BT.Enforcers
{
    public abstract class BTADecoratorNode : BTAExecutorNode
    {
        protected BTDecoratorNodeData DecoratorData { get; private set; }

        protected BTAExecutorNode ExecutorNode { get; private set; }

        public override void DoInitilize(BTController controller, BTNodeData data)
        {
            base.DoInitilize(controller, data);

            DecoratorData = data as BTDecoratorNodeData;
            if(DecoratorData!=null && DecoratorData.ExecutorData !=null)
            {

            }
        }

        public override void DoEnter()
        {
            base.DoEnter();
            ExecutorNode?.DoEnter();
        }

        public override void DoExit()
        {
            ExecutorNode?.DoExit();
            base.DoExit();
        }

        public override void DoDestroy()
        {
            ExecutorNode = null;
            DecoratorData = null;

            base.DoDestroy();
        }
    }
}
