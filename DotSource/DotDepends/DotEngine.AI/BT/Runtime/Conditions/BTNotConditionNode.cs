namespace DotEngine.AI.BT
{
    public class BTNotConditionNode : ABTConditionNode
    {
        public ABTConditionNode ConditionNode;

        public override bool IsMeet()
        {
            return !(ConditionNode == null ? false : ConditionNode.IsMeet());
        }
    }
}
