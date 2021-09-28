using System.Collections.Generic;

namespace DotEngine.AI.FSM.Editor
{
    public class FSMTranslateConditionData
    {
        public string toStateName;
        public CombineOperationType operationType = CombineOperationType.And;
        public List<ConditionData> datas = new List<ConditionData>();
    }

    public enum SingleConditionOperationType
    {
        Is = 0,
        Not,
    }

    public enum CombineOperationType
    {
        And,
        Or
    }

    public class ConditionData
    {
    }

    public class SingleConditionData : ConditionData
    {
        public SingleConditionOperationType operationType = SingleConditionOperationType.Is;
        public ComparedData comparedData = null;
    }

    public class CombineConditionData : ConditionData
    {
        public CombineOperationType operationType = CombineOperationType.And;
        public List<SingleConditionData> datas = new List<SingleConditionData>();
    }

    public class ComparedData
    {
        public string bbDataName;
        public int operationValue;
        public string extraValue;

        public string targetValue;
    }

    public enum NumberOperationType
    {
        Eq = 0,//==
        NEq,//!=
        GT,//>
        LT,//<
        GE,//>=
        LE,//<=
    }

    public enum StringOperationType
    {
        Null = 0,//null
        Empty,//""
        NullOrEmpty,//null or ""
        Equal,//==
        NotEqual,//!=
    }

    public enum BoolOperationType
    {
        True = 0,
        False,
    }

    public enum TableOperationType
    {
        Null = 0,
        NotNull,
        Statement,
    }
}
