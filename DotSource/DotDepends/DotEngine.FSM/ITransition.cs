namespace DotEngine.FSM
{
    public interface ITransition
    {
        string From { get; set; }
        string To { get; set; }
        ICondition Condition { get; set; }

        void DoInitilize(Machine machine);
        void DoDestroy();
    }
}
