namespace DotEngine.AI.BT
{
    public class BTOrConditionNode : ABTConditionNode
    {
        public ABTConditionNode ACondition;
        public ABTConditionNode BCondition;

        public override bool IsMeet()
        {
            return (ACondition == null ? false : ACondition.IsMeet())
                || (BCondition == null ? false : BCondition.IsMeet());
        }
    }
}
