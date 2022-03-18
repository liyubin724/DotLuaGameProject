namespace DotEngine.FSM
{
    public class FSAndCondition : IFSCondition
    {
        public IFSCondition ConditionA { get; set; }
        public IFSCondition ConditionB { get; set; }

        public bool IsSatisfy()
        {
            if(ConditionA == null || ConditionB == null)
            {
                return false;
            }

            return ConditionA.IsSatisfy() && ConditionB.IsSatisfy();
        }
    }
}
