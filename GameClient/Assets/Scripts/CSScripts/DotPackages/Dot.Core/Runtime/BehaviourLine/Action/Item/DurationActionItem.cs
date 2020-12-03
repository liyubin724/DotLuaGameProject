namespace DotEngine.BehaviourLine.Action
{
    public abstract class DurationActionItem : ActionItem
    {
        public float RealDurationTime { get; private set; }

        public float RealEndTime { get => RealFireTime + RealDurationTime; }

        protected DurationActionItem() : base()
        {

        }

        public override void SetData(LineContext context, ActionData data, float timeScale)
        {
            base.SetData(context, data, timeScale);
            RealDurationTime = ((DurationActionData)data).DurationTime * timeScale;
        }

        public abstract void DoEnter();
        public abstract void DoUpdate(float deltaTime);
        public abstract void DoExit();

        public virtual void DoPause() { }
        public virtual void DoResume() { }

        public override void DoReset()
        {
            base.DoReset();
            RealDurationTime = 0.0f;
        }

    }
}
