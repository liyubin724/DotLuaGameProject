using DotEngine.AI.BD.Conditions;

namespace DotEngine.AI.BD.Actions
{
    public abstract class ActionNode : Node
    {
        public int Index = -1;

        public ConditionNode Precondition = null;

        public float FireTime = 0.0f;

        public float TimeScale { get; set; } = 1.0f;
        public float RealFireTime => FireTime * TimeScale;
        public abstract void DoExecute();

        public virtual void DoReset()
        {
        }
    }
}
