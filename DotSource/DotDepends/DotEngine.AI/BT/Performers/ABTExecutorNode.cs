using DotEngine.AI.BT.Datas;

namespace DotEngine.AI.BT
{
    public abstract class ABTExecutorNode : ABTNode
    {
        protected ABTConditionNode ConditionNode { get; set; }

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
    }
}
