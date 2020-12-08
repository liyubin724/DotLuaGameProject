using UnityEngine;

namespace DotEngine.BD.Datas.Actions
{
    [ActionData("Debug", "Log", ActionCategory.Debug, Target = DataTarget.Client, Model = DataMode.Debug)]
    public class DebugLogData : EventActionData
    {
        public LogType Type = LogType.Log;
        public bool IsDetail = false;
        public string Message = string.Empty;
    }
}
