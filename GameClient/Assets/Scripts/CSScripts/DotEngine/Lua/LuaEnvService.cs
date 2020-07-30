using DotEngine.Log;
using DotEngine.Services;
using System;
using XLua;

namespace DotEngine.Lua
{
    public class LuaEnvService : Service,IUpdate
    {
        public const string NAME = "LuaService";

        private const string MGR_NAME = "EnvMgr";
        private const string PreloadScript = "Game/Startup";

        public float TickInterval { get; set; } = 0;
        public LuaEnv Env { get; private set; } = null;

        public LuaTable GameTable { get; private set; } = null;
        public LuaTable EnvTable { get; private set; } = null;

        private float elapsedTime = 0.0f;
        private ScriptLoader m_ScriptLoader = null;
        private Action<LuaTable, float> m_UpdateAction = null;

        public LuaEnvService() : base(NAME)
        {
        }

        public bool IsValid()
        {
            return Env != null && Env.IsValid();
        }

        public override void DoRegister()
        {
            Env = new LuaEnv();

            m_ScriptLoader = new FileScriptLoader(new string[] { LuaConst.GetScriptPathFormat() });
            Env.AddLoader(m_ScriptLoader.LoadScript);

            if (!RequireScript(PreloadScript))
            {
                LogUtil.LogError(LuaConst.LOGGER_NAME, "Load script failed. path = " + PreloadScript);
            }

            GameTable = Env.Global.Get<LuaTable>("Game");
            if(GameTable == null)
            {
                LogUtil.LogError(LuaConst.LOGGER_NAME, "the table which name game is not found. ");
            }

            EnvTable = GameTable.Get<LuaTable>(MGR_NAME);
            if(EnvTable == null)
            {
                LogUtil.LogError(LuaConst.LOGGER_NAME, "the table which name EnvMgr is not found. ");
            }else
            {
                m_UpdateAction = EnvTable.Get<Action<LuaTable, float>>(LuaConst.UPDATE_FUNCTION_NAME);
            }
        }

        public bool RequireScript(string scriptPath)
        {
            if(IsValid())
            {
                return LuaUtility.Require(Env, scriptPath);
            }
            return false;
        }

        public virtual void DoUpdate(float deltaTime)
        {
            if(!Env.IsValid())
            {
                return;
            }

            m_UpdateAction?.Invoke(EnvTable, deltaTime);

            if (TickInterval>0)
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

        public void CallAction(string funcName)
        {
            if(IsValid())
            {
                Action<LuaTable> action = EnvTable.Get<Action<LuaTable>>(funcName);
                action?.Invoke(EnvTable);
            }
        }

        public override void DoRemove()
        {
            DoDispose();
        }

        protected virtual void DoDispose()
        {
            m_UpdateAction = null;
            EnvTable?.Dispose();
            EnvTable = null;

            GameTable?.Dispose();
            GameTable = null;

            if (IsValid())
            {
                Env.Dispose();
                Env = null;
            }

            elapsedTime = 0.0f;
        }
    }
}
