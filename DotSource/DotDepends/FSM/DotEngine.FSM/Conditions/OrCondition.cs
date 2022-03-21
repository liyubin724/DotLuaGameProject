namespace DotEngine.FSM
{
    [CustomCondition(5, "Or", "")]
    public class OrCondition : ICondition
    {
        public string Guid { get; set; }
        public string DisplayName { get; set; }

        public ICondition ConditionA { get; set; }
        public ICondition ConditionB { get; set; }

        public void DoInitilize(Blackboard blackboard)
        {
            ConditionA?.DoInitilize(blackboard);
            ConditionB?.DoInitilize(blackboard);
        }

        public void DoDestroy()
        {
            ConditionA?.DoDestroy();
            ConditionB?.DoDestroy();
        }

        public bool IsSatisfy()
        {
            bool resultA = ConditionA == null ? false : ConditionA.IsSatisfy();
            bool resultB = ConditionB == null ? false : ConditionB.IsSatisfy();

            return resultA || resultB;
        }
    }
}
