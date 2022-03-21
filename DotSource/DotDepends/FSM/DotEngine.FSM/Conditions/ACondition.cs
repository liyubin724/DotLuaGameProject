namespace DotEngine.FSM
{
    public abstract class ACondition : ICondition
    {
        public string Guid{get;set;}
        public string DisplayName { get; set; }

        protected Blackboard Blackboard { get; set; }

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
