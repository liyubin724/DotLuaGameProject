using DotEngine.BD.Datas;
using System;

namespace DotEngine.BD.Executors
{
    public abstract class ActionExecutor : BDExecutor
    {
        public ActionData ActionData { get; private set; }

        public float FireTime => ActionData.FireTime;

        public T GetData<T>() where T:ActionData
        {
            return (T)Data;
        }

        public T GetContextData<T>()
        {
            return GetContextData<T>(typeof(T));
        }

        public T GetContextData<T>(Type objType)
        {
            var data = Context.Get(objType);
            if(data !=null)
            {
                return (T)data;
            }
            return default;
        }

        protected ActionExecutor() { }

        public override void DoInit(CutsceneContext context, BDData data)
        {
            base.DoInit(context, data);

            ActionData = (ActionData)data;
        }

        public override void DoReset()
        {
            ActionData = null;

            base.DoReset();
        }
    }
}
