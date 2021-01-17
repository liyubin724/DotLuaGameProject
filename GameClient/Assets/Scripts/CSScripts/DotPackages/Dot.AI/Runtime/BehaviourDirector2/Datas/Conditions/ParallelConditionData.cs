using System.Collections.Generic;

namespace DotEngine.BD.Datas
{
    [ConditionData("","Parallel")]
    public sealed class ParallelConditionData : ConditionData
    {
        public List<ConditionData> Childs = new List<ConditionData>();
    }
}
