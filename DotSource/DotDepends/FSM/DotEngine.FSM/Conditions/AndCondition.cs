namespace DotEngine.FSM
{
    [CustomCondition(3, "And", "")]
    public class AndCondition : ICondition
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
            if(ConditionA == null || ConditionB == null)
            {
                return false;
            }

            return ConditionA.IsSatisfy() && ConditionB.IsSatisfy();
        }
    }
}
