namespace DotEngine.FSM
{
    public abstract class AFSCondition : IFSCondition
    {
        public FSBlackboard Blackboard { get; set; }

        public abstract bool IsSatisfy();
    }
}
