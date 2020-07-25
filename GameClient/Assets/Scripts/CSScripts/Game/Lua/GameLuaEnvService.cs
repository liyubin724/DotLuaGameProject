using DotEngine.Log;
using DotEngine.Lua;
using System;
using XLua;

namespace Game.Lua
{
    public class GameLuaEnvService : LuaEnvService
    {
        private const string MGR_NAME = "EnvMgr";
        private const string GLOBAL_GAME_NAME = "Game";
        private const string IS_DEBUG_FIELD_NAME = "IsDebug";

        private string[] m_ScriptFormats = null;
        private string[] m_PreloadScripts = null;
        private string m_MgrScript = null;
        private ScriptLoader m_ScriptLoader = null;
        private LuaTable m_EnvMgr = null;
        private Action<LuaTable, float> m_UpdateAction = null;

        public LuaTable GlobalGameTable { get; private set; }

        public GameLuaEnvService(string[] scriptFormats,string[] preloadScripts,string mgrScript)
        {
            m_ScriptFormats = scriptFormats;
            m_PreloadScripts = preloadScripts;
            m_MgrScript = mgrScript;
        }

        protected override void InitEnv()
        {
            base.InitEnv();

            m_ScriptLoader = new FileScriptLoader(m_ScriptFormats);
            Env.AddLoader(m_ScriptLoader.LoadScript);

            GlobalGameTable = Env.NewTable();
            Env.Global.Set(GLOBAL_GAME_NAME, GlobalGameTable);
#if DEBUG
            GlobalGameTable.Set(IS_DEBUG_FIELD_NAME, true);
#endif

            if (m_PreloadScripts != null && m_PreloadScripts.Length > 0)
            {
                for (int i = 0; i < m_PreloadScripts.Length; ++i)
                {
                    if (!RequireScript(m_PreloadScripts[i]))
                    {
                        LogUtil.LogError(LuaConst.LOGGER_NAME, "Load script failed. path = " + m_PreloadScripts[i]);
                    }
                }
            }

            if (!string.IsNullOrEmpty(m_MgrScript))
            {
                m_EnvMgr = LuaUtility.Instance(Env, m_MgrScript);
                if (m_EnvMgr == null)
                {
                    LogUtil.LogError(LuaConst.LOGGER_NAME, "LuaEnvService::CreateEnv->Load mgr Failed.mgrScriptPath = " + m_MgrScript);
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

        public override void DoUpdate(float deltaTime)
        {
            base.DoUpdate(deltaTime);
            m_UpdateAction?.Invoke(m_EnvMgr, deltaTime);
        }

        protected override void DoDispose()
        {
            m_UpdateAction = null;
            m_EnvMgr?.Dispose();
            m_EnvMgr = null;

            GlobalGameTable?.Dispose();
            GlobalGameTable = null;

            base.DoDispose();
        }
    }
}
