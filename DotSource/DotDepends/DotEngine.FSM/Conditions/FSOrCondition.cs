namespace DotEngine.FSM
{
    public class FSOrCondition : IFSCondition
    {
        public IFSCondition ConditionA { get; set; }
        public IFSCondition ConditionB { get; set; }

        public bool IsSatisfy()
        {
            bool resultA = ConditionA == null ? false : ConditionA.IsSatisfy();
            bool resultB = ConditionB == null ? false : ConditionB.IsSatisfy();

            return resultA || resultB;
        }
    }
}
