using UnityEngine;

namespace DotEngine.BD.Datas
{
    [ActionData("Debug", "Log", ActionCategory.Debug, Target = BDDataTarget.Client, Model = BDDataMode.Debug)]
    public class DebugLogData : EventActionData
    {
        public LogType LogType = LogType.Log;
        public string Message = string.Empty;
    }
}
