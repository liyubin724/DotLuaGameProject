namespace DotEngine.FSM
{
    public interface ICondition
    {
        string Guid { get; set; }
        string DisplayName { get; set; }

        void DoInitilize(Blackboard blackboard);
        bool IsSatisfy();
        void DoDestroy();
    }
}
