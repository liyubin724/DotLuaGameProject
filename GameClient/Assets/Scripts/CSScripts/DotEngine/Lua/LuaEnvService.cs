using DotEngine.Services;
using XLua;

namespace DotEngine.Lua
{
    public class LuaEnvService : Service,IUpdate
    {
        public const string NAME = "LuaService";

        public float TickInterval { get; set; } = 0;
        public LuaEnv Env { get; private set; } = null;

        private float elapsedTime = 0.0f;

        public LuaEnvService() : base(NAME)
        {
        }

        public bool IsValid()
        {
            return Env != null && Env.IsValid();
        }

        public override void DoRegister()
        {
            InitEnv();
        }

        protected virtual void InitEnv()
        {
            Env = new LuaEnv();
#if DEBUG
            Env.Global.Set(LuaConst.IS_DEBUG_FIELD_NAME, true);
#endif

        }

        public bool RequireScript(string scriptPath)
        {
            if(IsValid())
            {
                return LuaUtility.Require(Env, scriptPath);
            }
            return false;
        }

        public LuaTable InstanceScript(string scriptPath)
        {
            if(IsValid())
            {
                return LuaUtility.Instance(Env, scriptPath);
            }
            return null;
        }

        public virtual void DoUpdate(float deltaTime)
        {
            if(!Env.IsValid())
            {
                return;
            }

            if(TickInterval>0)
            {
                elapsedTime += deltaTime;
                if(elapsedTime>=TickInterval)
                {
                    elapsedTime -= TickInterval;
                    Env.Tick();
                }
            }
        }

        public void FullGC()
        {
            if(IsValid())
            {
                Env.FullGc();
            }
        }

        public override void DoRemove()
        {
            DoDispose();
        }

        protected virtual void DoDispose()
        {
            if (IsValid())
            {
                Env.Dispose();
                Env = null;
            }

            elapsedTime = 0.0f;
        }
    }
}
