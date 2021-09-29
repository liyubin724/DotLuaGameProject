using System.Collections.Generic;

namespace DotEngine.AI.FSM.Editor
{
    public class FSMTransition
    {
        public string from;
        public string to;

        public FSMCombineCondition condition;
    }

    public enum ConditionOperationType
    {
        And,
        Or,
        Is,
        Not,
    }

    public class FSMCondition
    {
        public ConditionOperationType operationType = ConditionOperationType.And;
    }

    public class FSMCombineCondition:FSMCondition
    {
        public List<FSMCondition> conditions = new List<FSMCondition>();
    }

    public class FSMSingleCondition:FSMCondition
    {
        public string bbName;

        public int compareType;
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
