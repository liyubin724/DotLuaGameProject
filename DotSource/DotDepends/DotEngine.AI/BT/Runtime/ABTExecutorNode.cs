namespace DotEngine.AI.BT
{
    public abstract class ABTExecutorNode : ABTNode
    {
        public virtual bool CanExecute()
        {
            return true;
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
