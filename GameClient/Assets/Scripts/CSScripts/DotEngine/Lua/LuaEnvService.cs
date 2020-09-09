﻿using DotEngine.Log;
using DotEngine.Services;
using System;
using XLua;

namespace DotEngine.Lua
{
    public class LuaEnvService : Service,IUpdate,IUnscaleUpdate,ILateUpdate,IFixedUpdate
    {
        public const string NAME = "LuaService";

        private const string MGR_NAME = "Game";
        private const string PreloadScript = "Game/Startup";

        public float TickInterval { get; set; } = 0;
        public LuaEnv Env { get; private set; } = null;
        public LuaTable GameTable { get; private set; } = null;

        private float elapsedTime = 0.0f;
        private ScriptLoader m_ScriptLoader = null;

        private Action<float> m_UpdateAction = null;
        private Action<float> m_UnscaleUpdateAction = null;
        private Action<float> m_LateUpdateAction = null;
        private Action<float> m_FixedUpdateAction = null;

        private Func<string, LuaTable> m_InstanceFunc = null;

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
                return;
            }

            GameTable = Env.Global.Get<LuaTable>(MGR_NAME);
            if(GameTable == null)
            {
                LogUtil.LogError(LuaConst.LOGGER_NAME, "the table which name game is not found. ");
            }else
            {
                m_UpdateAction = GameTable.Get<Action<float>>(LuaConst.UPDATE_FUNCTION_NAME);
                m_UnscaleUpdateAction = GameTable.Get<Action<float>>(LuaConst.UNSCALEUPDATE_FUNCTION_NAME);
                m_LateUpdateAction = GameTable.Get<Action<float>>(LuaConst.LATEUPDATE_FUNCTION_NAME);
                m_FixedUpdateAction = GameTable.Get<Action<float>>(LuaConst.FIXEDUPDATE_FUNCTION_NAME);
            }
        }

        public bool RequireScript(string scriptPath)
        {
            if(IsValid())
            {
                if (string.IsNullOrEmpty(scriptPath))
                {
                    LogUtil.LogError(LuaConst.LOGGER_NAME, "script is empty");
                    return false;
                }
                string scriptName = GetScriptName(scriptPath);
                if (string.IsNullOrEmpty(scriptName))
                {
                    LogUtil.LogError(LuaConst.LOGGER_NAME, "scriptName is empty");
                    return false;
                }

                if (!Env.Global.ContainsKey(scriptName))
                {
                    Env.DoString(string.Format("require (\"{0}\")", scriptPath));
                }
                return true;
            }
            return false;
        }

        private string GetScriptName(string script)
        {
            if (string.IsNullOrEmpty(script))
            {
                LogUtil.LogError(LuaConst.LOGGER_NAME, "script is empty");
                return null;
            }

            string scriptName = script;
            int index = script.LastIndexOf("/");
            if (index > 0)
            {
                scriptName = script.Substring(index + 1);
            }
            return scriptName;
        }

        public LuaTable InstanceScript(string scriptPath)
        {
            if(m_InstanceFunc == null)
            {
                m_InstanceFunc = Env.Global.Get<Func<string, LuaTable>>("instance");
            }

            return m_InstanceFunc(scriptPath);
        }

        public virtual void DoUpdate(float deltaTime)
        {
            if(!Env.IsValid())
            {
                return;
            }

            m_UpdateAction?.Invoke(deltaTime);

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

        public void DoUnscaleUpdate(float deltaTime)
        {
            if (!Env.IsValid())
            {
                return;
            }
            m_UnscaleUpdateAction?.Invoke(deltaTime);
        }

        public void DoFixedUpdate(float deltaTime)
        {
            if (!Env.IsValid())
            {
                return;
            }
            m_FixedUpdateAction?.Invoke(deltaTime);
        }

        public void DoLateUpdate(float deltaTime)
        {
            if (!Env.IsValid())
            {
                return;
            }
            m_LateUpdateAction?.Invoke(deltaTime);
        }

        public void FullGC()
        {
            if(IsValid())
            {
                Env.FullGc();
            }
        }
        //返回 Lua 使用的内存总量（以 K 字节为单位）
        public float GetTotalMemory()
        {
            if(IsValid())
            {
                return Env.GetTotalMemory();
            }
            return 0.0f;
        }

        public void CallAction(string funcName)
        {
            if(IsValid())
            {
                Action action = GameTable.Get<Action>(funcName);
                action?.Invoke();
            }
        }

        public override void DoRemove()
        {
            CallAction(LuaConst.DESTROY_FUNCTION_NAME);

            DoDispose();
        }

        protected virtual void DoDispose()
        {
            m_InstanceFunc = null;

            m_UpdateAction = null;
            m_UnscaleUpdateAction = null;
            m_LateUpdateAction = null;
            m_FixedUpdateAction = null;

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
