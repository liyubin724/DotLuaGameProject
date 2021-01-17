using System.Collections.Generic;

namespace DotEngine.BehaviourDirector.Nodes
{
    [ConditionNode("", "Sequence")]
    public class SequenceConditionNode : ConditionNode
    {
        public List<ConditionNode> Conditions = new List<ConditionNode>();
    }
}
