using DotEngine.AI.BT.Datas;

namespace DotEngine.AI.BT.Enforcers
{
    public abstract class BTAExecutorNode : BTANode
    {
        protected BTExecutorNodeData ExecutorData { get; private set; }
        protected BTAConditionNode ConditionNode { get; set; }

        public override void DoInitilize(BTController controller, BTNodeData data)
        {
            base.DoInitilize(controller, data);
            ExecutorData = data as BTExecutorNodeData;

            if(ExecutorData!=null && ExecutorData.ConditionData!=null)
            {

            }
        }

        public virtual bool CanExecute()
        {
            return ConditionNode == null ? true : ConditionNode.IsMeet();
        }

        public virtual void DoEnter()
        {
            Controller?.PushNode(this);
        }

        public abstract EBTResult DoExecute(float deltaTime);

        public virtual void DoExit()
        {
            Controller?.PopNode(this);
        }

        public override void DoDestroy()
        {
            ConditionNode = null;
            ExecutorData = null;
            base.DoDestroy();
        }
    }
}
