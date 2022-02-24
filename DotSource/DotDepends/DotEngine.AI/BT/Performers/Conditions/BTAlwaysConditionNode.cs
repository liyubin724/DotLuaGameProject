namespace DotEngine.AI.BT
{
    public class BTAlwaysConditionNode : ABTConditionNode
    {
        private bool value;

        public override bool IsMeet()
        {
            return value;
        }
    }
}
