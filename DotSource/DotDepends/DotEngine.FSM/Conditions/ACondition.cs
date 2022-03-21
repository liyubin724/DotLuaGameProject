namespace DotEngine.FSM
{
    public abstract class ACondition : ICondition
    {
        public Blackboard Blackboard { get; set; }

        public virtual void DoInitilize(Blackboard blackboard)
        {
            Blackboard = blackboard;
        }

        public virtual void DoDestroy()
        {
            Blackboard = null;
        }

        public abstract bool IsSatisfy();
    }
}
