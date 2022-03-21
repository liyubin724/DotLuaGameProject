namespace DotEngine.FSM
{
    public class AlwaysFalseCondition : ACondition
    {
        public override bool IsSatisfy()
        {
            return false;
        }
    }
}
