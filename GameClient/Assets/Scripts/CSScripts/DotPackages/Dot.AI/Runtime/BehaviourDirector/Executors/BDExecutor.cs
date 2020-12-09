using DotEngine.BD.Datas;

namespace DotEngine.BD.Executors
{
    public abstract class BDExecutor
    {
        public CutsceneContext Context { get; private set; }
        
        public BDData Data { get; private set; }

        public virtual void DoInit(CutsceneContext context,BDData data)
        {
            Context = context;
            Data = data;
        }

        public virtual void DoReset()
        {
            Context = null;
            Data = null;
        }
    }
}
