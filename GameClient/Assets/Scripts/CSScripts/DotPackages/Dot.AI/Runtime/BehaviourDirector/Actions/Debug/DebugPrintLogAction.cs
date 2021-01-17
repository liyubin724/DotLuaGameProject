using UnityEngine;

namespace DotEngine.BehaviourDirector.Nodes
{
    [ActionNode("Events/Debug", "Print Log", ActionNodeCategory.Debug, Model = NodeMode.Debug, Platform = NodePlatform.Client)]
    public class DebugPrintLogAction : EventActionNode
    {
        public LogType Type = LogType.Log;
        public string Message = string.Empty;

        public override void DoTrigger()
        {
            
        }
    }
}
