using DotEngine.AI.BD.Conditions;

namespace DotEngine.AI.BD.Actions
{
    public enum ActionCategory
    {

    }

    public abstract class Action
    {
        public ConditionNode Precondition = null;

        public float FireTime = 0.0f;

        public float TimeScale { get; set; } = 1.0f;
        public float RealFireTime => FireTime * TimeScale;

        public virtual void DoInit() { }
        public virtual void DoReset() { }
    }
}
