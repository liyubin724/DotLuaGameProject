using System.Collections.Generic;

namespace DotEngine.BehaviourDirector.Nodes
{
    [ConditionNode("","Parallel")]
    public class ParallelConditionNode : ConditionNode
    {
        public List<ConditionNode> Conditions = new List<ConditionNode>();
    }
}
