using System.Collections.Generic;

namespace DotEngine.BL.Node.Condition
{
    public class SequenceConditionData : ConditionData
    {
        public List<ConditionData> Childs { get; } = new List<ConditionData>();
    }
}
