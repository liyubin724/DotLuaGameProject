namespace DotEngine.AI.BT
{
    public class BTAndConditionNode : ABTConditionNode
    {
        public ABTConditionNode ACondition;
        public ABTConditionNode BCondition;

        public override bool IsMeet()
        {
            return (ACondition == null ? false : ACondition.IsMeet()) 
                && (BCondition == null ? false : BCondition.IsMeet());
        }
    }
}
