namespace DotEngine.FSM
{
    [CustomCondition(2, "Always False", "")]
    public class AlwaysFalseCondition : ACondition
    {
        public override bool IsSatisfy()
        {
            return false;
        }
    }
}
