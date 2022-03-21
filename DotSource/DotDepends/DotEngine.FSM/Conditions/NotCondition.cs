namespace DotEngine.FSM
{
    public class NotCondition : ICondition
    {
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
