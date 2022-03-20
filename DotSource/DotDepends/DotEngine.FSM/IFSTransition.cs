namespace DotEngine.FSM
{
    public interface IFSTransition
    {
        string From { get; set; }
        string To { get; set; }
        IFSCondition Condition { get; set; }

        void DoInitilize(FSMachine machine);
        void DoDestroy();
    }
}
