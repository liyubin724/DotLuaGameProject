using DotEngine.Log;
using DotEngine.Services;
using System;
using System.Collections.Generic;
using XLua;
using SystemObject = System.Object;

namespace DotEngine.Lua
{
    public class LuaEnvService : Servicer,IUpdate,ILateUpdate,IFixedUpdate
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

        private LuaFunction m_InstanceFunc = null;
        private LuaFunction m_InstanceWithFunc = null;

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
                LogUtil.Error(LuaConst.LOGGER_NAME, "Load script failed. path = " + PreloadScript);
                return;
            }

            m_InstanceFunc = Env.Global.Get<LuaFunction>("instance");
            m_InstanceWithFunc = Env.Global.Get<LuaFunction>("instancewith");

            GameTable = Env.Global.Get<LuaTable>(MGR_NAME);
            if(GameTable == null)
            {
                LogUtil.Error(LuaConst.LOGGER_NAME, "the table which name game is not found. ");
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
                    LogUtil.Error(LuaConst.LOGGER_NAME, "script is empty");
                    return false;
                }
                string scriptName = GetScriptName(scriptPath);
                if (string.IsNullOrEmpty(scriptName))
                {
                    LogUtil.Error(LuaConst.LOGGER_NAME, "scriptName is empty");
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
                LogUtil.Error(LuaConst.LOGGER_NAME, "script is empty");
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
            return m_InstanceFunc.Func<string,LuaTable>(scriptPath);
        }

        public LuaTable InstanceScriptWith(string scriptPath,LuaOperateParam[] operateParams)
        {
            List<SystemObject> list = new List<SystemObject>();
            list.Add(scriptPath);
            if(operateParams!=null && operateParams.Length>0)
            {
                foreach(var p in operateParams)
                {
                    list.Add(p.GetValue());
                }
            }
            return m_InstanceWithFunc.Func<LuaTable>(list.ToArray());
        }

        public virtual void DoUpdate(float deltaTime,float unscaleDeltaTime)
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

            m_UnscaleUpdateAction?.Invoke(unscaleDeltaTime);
        }

        public void DoFixedUpdate(float deltaTime,float unscaleDeltaTime)
        {
            if (!Env.IsValid())
            {
                return;
            }
            m_FixedUpdateAction?.Invoke(deltaTime);
        }

        public void DoLateUpdate(float deltaTime,float unscaleDeltaTime)
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
