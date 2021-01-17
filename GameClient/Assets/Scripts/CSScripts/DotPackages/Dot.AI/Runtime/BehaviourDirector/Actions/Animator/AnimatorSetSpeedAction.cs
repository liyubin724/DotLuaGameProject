namespace DotEngine.BehaviourDirector.Nodes
{
    [ActionNode("Events/Animator", "Set Speed", ActionNodeCategory.Animator, Platform = NodePlatform.Client)]
    public class AnimatorSetSpeedAction : EventActionNode
    {
        public float Speed = 1.0f;

        public override void DoTrigger()
        {
            throw new System.NotImplementedException();
        }
    }
}
