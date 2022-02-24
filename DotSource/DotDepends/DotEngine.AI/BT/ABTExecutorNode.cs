namespace DotEngine.AI.BT
{
    public abstract class ABTExecutorNode : ABTNode
    {
        public abstract bool IsValid();

        public abstract void DoEnter();
        public abstract EBTResult DoExecute(float deltaTime);
        public abstract void DoExit();
    }
}
