using System.Collections.Generic;

namespace DotEngine.BD.Datas
{
    [ConditionData("", "Parallel")]
    public sealed class SequenceConditionData : ConditionData
    {
        public List<ConditionData> Childs = new List<ConditionData>();
    }
}
