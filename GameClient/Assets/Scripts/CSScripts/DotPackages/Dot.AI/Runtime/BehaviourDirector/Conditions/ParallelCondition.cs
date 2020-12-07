using System.Collections.Generic;

namespace DotEngine.AI.BD.Conditions
{
    [NodeMenu("Parallel","")]
    [NodeUsage(NodeCategory.Condition,NodePlatform.All)]
    public sealed class ParallelCondition : ConditionNode
    {
        public List<ConditionNode> Childs = new List<ConditionNode>();

        public void AddCondition(ConditionNode node)
        {
            Childs.Add(node);
        }

        public override bool IsMet()
        {
            foreach(var child in Childs)
            {
                if(child.IsMet())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
