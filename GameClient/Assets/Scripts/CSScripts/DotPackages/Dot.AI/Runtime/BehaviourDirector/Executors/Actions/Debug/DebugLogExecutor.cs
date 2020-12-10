using DotEngine.BD.Datas;
using UnityEngine;

namespace DotEngine.BD.Executors
{
    [BDExecutor(typeof(DebugLogEvent))]
    public class DebugLogExecutor : EventActionExecutor
    {
        public override void DoTrigger()
        {
            DebugLogEvent logData = GetData<DebugLogEvent>();

            if(logData.LogType == LogType.Log)
            {
                Debug.Log(logData.Message);
            }else if(logData.LogType == LogType.Warning)
            {
                Debug.LogWarning(logData.Message);
            }else if(logData.LogType == LogType.Error || logData.LogType == LogType.Exception)
            {
                Debug.LogError(logData.Message);
            }
        }
    }
}
