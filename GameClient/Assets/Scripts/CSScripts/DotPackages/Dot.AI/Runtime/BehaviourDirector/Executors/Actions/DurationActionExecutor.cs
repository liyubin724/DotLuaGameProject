using DotEngine.BD.Datas;

namespace DotEngine.BD.Executors
{
    public abstract class DurationActionExecutor : ActionExecutor
    {
        protected DurationActionData DurationData { get; private set; }

        public float DurationTime => DurationData.DurationTime;
        public float EndTime => FireTime + DurationTime;

        public override void DoInit(CutsceneContext context, BDData data)
        {
            base.DoInit(context, data);

            DurationData = (DurationActionData)data;
        }

        public abstract void DoEnter();
        public abstract void DoUpdate(float deltaTime);
        public abstract void DoExit();

        public virtual void DoPause() { }
        public virtual void DoResume() { }

        public override void DoReset()
        {
            DurationData = null;
            base.DoReset();
        }
    }
}
