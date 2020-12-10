using UnityEngine;

namespace DotEngine.BD.Datas
{
    [ActionData("Animator/Events", "Log", ActionCategory.Debug, Target = BDDataTarget.Client, Model = BDDataMode.Debug)]
    public class DebugLogEvent : EventActionData
    {
        public LogType LogType = LogType.Log;
        public string Message = string.Empty;
    }
}
