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
        private const string IS_DEBUG_FIELD_NAME = "IsDebug";

        private static string[] PreloadScripts = new string[]
        {
            "Game/Startup"
        };
        private static string MgrScript = "Game/EnvManager";

        public float TickInterval { get; set; } = 0;
        public LuaEnv Env { get; private set; } = null;

        public LuaTable GlobalGameTable { get; private set; }

        private float elapsedTime = 0.0f;
        private ScriptLoader m_ScriptLoader = null;
        private LuaTable m_EnvMgr = null;
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

            GlobalGameTable = Env.NewTable();
            Env.Global.Set(LuaConst.GLOBAL_GAME_NAME, GlobalGameTable);
#if DEBUG
            GlobalGameTable.Set(IS_DEBUG_FIELD_NAME, true);
#endif

            if (PreloadScripts != null && PreloadScripts.Length > 0)
            {
                for (int i = 0; i < PreloadScripts.Length; ++i)
                {
                    if (!RequireScript(PreloadScripts[i]))
                    {
                        LogUtil.LogError(LuaConst.LOGGER_NAME, "Load script failed. path = " + PreloadScripts[i]);
                    }
                }
            }

            if (!string.IsNullOrEmpty(MgrScript))
            {
                m_EnvMgr = LuaUtility.Instance(Env, MgrScript);
                if (m_EnvMgr == null)
                {
                    LogUtil.LogError(LuaConst.LOGGER_NAME, "LuaEnvService::CreateEnv->Load mgr Failed.mgrScriptPath = " + MgrScript);
                }
                else
                {
                    Action<LuaTable> luaMgrStartAction = m_EnvMgr.Get<Action<LuaTable>>(LuaConst.START_FUNCTION_NAME);
                    luaMgrStartAction?.Invoke(m_EnvMgr);

                    m_UpdateAction = m_EnvMgr.Get<Action<LuaTable, float>>(LuaConst.UPDATE_FUNCTION_NAME);

                    GlobalGameTable.Set(MGR_NAME, m_EnvMgr);
                }
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

            m_UpdateAction?.Invoke(m_EnvMgr, deltaTime);

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

        public override void DoRemove()
        {
            DoDispose();
        }

        protected virtual void DoDispose()
        {
            m_UpdateAction = null;
            m_EnvMgr?.Dispose();
            m_EnvMgr = null;

            GlobalGameTable?.Dispose();
            GlobalGameTable = null;

            if (IsValid())
            {
                Env.Dispose();
                Env = null;
            }

            elapsedTime = 0.0f;
        }
    }
}
