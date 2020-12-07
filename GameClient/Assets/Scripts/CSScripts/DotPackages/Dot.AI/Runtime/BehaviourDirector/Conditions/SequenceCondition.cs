using System.Collections.Generic;

namespace DotEngine.AI.BD.Conditions
{
    [NodeMenu("Sequence", "")]
    [NodeUsage(NodeCategory.Condition, NodePlatform.All)]
    public sealed class SequenceCondition : ConditionNode
    {
        public List<ConditionNode> Childs = new List<ConditionNode>();

        public override bool IsMet()
        {
            if(Childs.Count == 0)
            {
                return false;
            }

            foreach(var child in Childs)
            {
                if(!child.IsMet())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
