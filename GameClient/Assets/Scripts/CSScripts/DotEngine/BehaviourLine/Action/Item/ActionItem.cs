namespace DotEngine.BehaviourLine.Action
{
    public abstract class ActionItem
    {
        public ActionData Data { get; private set; }
        public float RealFireTime { get; private set; }

        protected LineContext Context { get; private set; }

        protected ActionItem() 
        {
        }

        public T GetData<T>() where T: ActionData
        {
            return (T)Data;
        }

        public virtual void SetData(LineContext context, ActionData data,float timeScale)
        {
            Context = context;
            Data = data;

            RealFireTime = Data.FireTime * timeScale;
        }

        public virtual void DoReset()
        {
            Context = null;
            Data = null;
            RealFireTime = 0.0f;
        }
    }
}
