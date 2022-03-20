using System.Collections.Generic;

namespace DotEngine.FSM
{
    public class FSData
    {
        public FSMachineData MachineData;
        public List<FSStateData> StateDatas = new List<FSStateData>();
        public List<FSTransitionData> TransitionDatas = new List<FSTransitionData>();
        public List<FSConditionData> ConditionDatas = new List<FSConditionData>();
    }

    public class FSMachineData
    {
        public string Name;
        public string Desc;
        public string InitState;
        public bool AutoRunWhenInitlized;
    }

    public class FSStateData
    {
        public string Guid;
        
        public string Name;
        public string Desc;
        public int ClassId;
    }

    public class FSTransitionData
    {
        public string Guid;
        
        public string Name;
        public string Desc;
        public string FromState;
        public string ToState;
        public string ConditionGuid;
        public int ClassId;
    }

    public class FSConditionData
    {
        public string Guid;
        public string Name;
        public string Desc;
        public int ClassId;

        public List<string> subConditionFieldNames;
        public List<string> subConditionGuids;
    }
}
