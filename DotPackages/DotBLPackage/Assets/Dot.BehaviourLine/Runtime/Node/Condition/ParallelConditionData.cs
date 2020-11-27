using System.Collections.Generic;

namespace DotEngine.BL.Node.Condition
{
    public class ParallelConditionData : ConditionData
    {
        public List<ConditionData> Childs { get; } = new List<ConditionData>();
    }
}
