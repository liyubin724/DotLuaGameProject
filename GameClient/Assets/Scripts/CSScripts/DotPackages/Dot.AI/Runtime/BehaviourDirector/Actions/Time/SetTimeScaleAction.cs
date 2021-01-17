using UnityEngine;

namespace DotEngine.BehaviourDirector.Nodes
{
    [ActionNode("Events/Time", "Set Time Scale", ActionNodeCategory.Time)]
    public class SetTimeScaleAction : EventActionNode
    {
        public float TimeScale = 1.0f;

        public override void DoTrigger()
        {
            Time.timeScale = TimeScale;
        }
    }
}
