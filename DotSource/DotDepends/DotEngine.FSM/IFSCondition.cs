namespace DotEngine.FSM
{
    public interface IFSCondition
    {
        void DoInitilize(FSBlackboard blackboard);
        bool IsSatisfy();
        void DoDestroy();
    }
}
