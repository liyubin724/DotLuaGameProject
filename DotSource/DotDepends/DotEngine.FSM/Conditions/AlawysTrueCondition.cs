namespace DotEngine.FSM
{
    [CustomCondition(1,"Always True","")]
    public class AlawysTrueCondition : ACondition
    {
        public override bool IsSatisfy()
        {
            return true;
        }
    }
}
