using System.Collections.Generic;

namespace DotEngine.FSM
{
    public class AssetData
    {
        public HeaderData HeaderData;
        public List<MachineData> MachineDatas = new List<MachineData>();
        public List<StateData> StateDatas = new List<StateData>();
        public List<TransitionData> TransitionDatas = new List<TransitionData>();
        public List<ConditionData> ConditionDatas = new List<ConditionData>();
    }

    public class HeaderData
    {
        public string RootMachineGuid;
    }

    public class MachineData
    {
        public string Guid;

        public string DisplayName;

        public string InitStateGuid;
        public bool AutoRunWhenInitlized;
        public List<string> stateGuids = new List<string>();
        public List<string> transitionGuids = new List<string>();
    }

    public class StateData
    {
        public string Guid;
        
        public string DisplayName;
        public int ClassId;
    }

    public class TransitionData
    {
        public string Guid;
        
        public string DisplayName;
        public int ClassId;

        public string FromStateGuid;
        public string ToStateGuid;
        public string ConditionGuid;
    }

    public class ConditionData
    {
        public string Guid;

        public string DisplayName;
        public int ClassId;

        public List<string> fieldNames = new List<string>();
        public List<string> valueGuids = new List<string>();
    }
}
