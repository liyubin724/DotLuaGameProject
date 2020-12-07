namespace DotEngine.AI.BD.Actions
{
    public abstract class DurationAction : ActionNode
    {
        public float Duration = 0.0f;
        public bool IsFixedDuration = false;

        public float EndTime => FireTime + Duration;
        public float RealEndTime => TimeScale * EndTime;

        public abstract void DoEnter();
        public abstract void DoUpdate(float deltaTime);
        public abstract void DoExit();

        public virtual void DoPause() { }
        public virtual void DoResume() { }
    }
}
