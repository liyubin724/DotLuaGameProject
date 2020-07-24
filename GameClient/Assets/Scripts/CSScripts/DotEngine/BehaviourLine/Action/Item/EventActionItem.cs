namespace DotEngine.BehaviourLine.Action
{
    public abstract class EventActionItem : ActionItem
    {
        protected EventActionItem():base()
        {

        }

        public abstract void DoTrigger();
    }
}
