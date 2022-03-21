namespace DotEngine.FSM
{
    public interface ICondition
    {
        void DoInitilize(Blackboard blackboard);
        bool IsSatisfy();
        void DoDestroy();
    }
}
