namespace DotEngine.AI.BD.Actions
{
    public abstract class EventAction : ActionNode
    {
        public virtual void DoTrigger()
        {
            DoExecute();
        }
    }
}
