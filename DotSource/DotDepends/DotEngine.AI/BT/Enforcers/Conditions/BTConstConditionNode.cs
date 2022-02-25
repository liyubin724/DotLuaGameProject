using DotEngine.AI.BT.Datas;

namespace DotEngine.AI.BT.Enforcers
{
    public class BTConstConditionNode : BTAConditionNode
    {
        public override bool IsMeet()
        {
            if(ConditionData == null)
            {
                return false;
            }

            return (ConditionData as BTConstConditionData).IsTrue;
        }
    }
}
