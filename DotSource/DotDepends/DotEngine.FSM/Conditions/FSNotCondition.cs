namespace DotEngine.FSM
{
    public class FSNotCondition : IFSCondition
    {
        public IFSCondition Condition { get; set; }

        public bool IsSatisfy()
        {
            bool result = Condition == null ? false : Condition.IsSatisfy();
            return !result;
        }
    }
}
