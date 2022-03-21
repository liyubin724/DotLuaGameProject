namespace DotEngine.FSM
{
    [CustomCondition(4, "Not", "")]
    public class NotCondition : ICondition
    {
        public string Guid { get; set; }
        public string DisplayName { get; set; }

        public ICondition Condition { get; set; }

        public void DoInitilize(Blackboard blackboard)
        {
            Condition?.DoInitilize(blackboard);
        }

        public void DoDestroy()
        {
            Condition?.DoDestroy();
        }


        public bool IsSatisfy()
        {
            bool result = Condition == null ? false : Condition.IsSatisfy();
            return !result;
        }
    }
}
