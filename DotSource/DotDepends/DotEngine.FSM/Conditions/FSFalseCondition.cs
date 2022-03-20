namespace DotEngine.FSM
{
    [CustomFSCondition("Always False", "")]
    public class FSFalseCondition : IFSCondition
    {
        public bool IsSatisfy()
        {
            return false;
        }
    }
}
