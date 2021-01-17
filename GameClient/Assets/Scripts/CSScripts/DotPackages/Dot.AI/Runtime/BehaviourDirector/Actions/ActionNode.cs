namespace DotEngine.BehaviourDirector.Nodes
{
    public enum ActionNodeCategory
    {
        None = 0,
        Debug,
        Camera,
        Audio,
        Animator,
        Transform,
        GameObject,
        Behaviour,
        Time,
    }

    public abstract class ActionNode : Node
    {
        public ConditionNode PreCondition = null;
        public float FireTime = 0.0f;
    }

    public abstract class EventActionNode : ActionNode
    {
        public abstract void DoTrigger();
    }

    public abstract class DurationActionNode : ActionNode
    {
        public float DurationTime = 0.0f;

        public abstract void DoEnter();
        public abstract void DoUpdate(float deltaTime);
        public abstract void DoExit();

        public virtual void DoPaused() { }
        public virtual void DoResume() { }
    }
}
