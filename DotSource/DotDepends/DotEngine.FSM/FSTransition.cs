namespace DotEngine.FSM
{
    public class FSTransition
    {
        public string From { get; set; }
        public string To { get; set; }
        public IFSCondition Condition { get; set; }
    }
}
