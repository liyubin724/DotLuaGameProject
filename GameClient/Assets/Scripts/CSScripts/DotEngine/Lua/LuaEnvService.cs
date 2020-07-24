using DotEngine.Log;
using DotEngine.Services;
using System;
using System.Collections.Generic;
using XLua;

namespace DotEngine.Lua
{
    internal class LuaEnvData
    {
        public LuaEnv Env { get; private set; }
        public ScriptLoader Loader { get; private set; }
        public LuaTable MgrTable { get; private set; }

        private Action<LuaTable, float> updateAction = null;

        public LuaEnvData(LuaEnv luaEnv, ScriptLoader loader)
        {
            Env = luaEnv;
            if (luaEnv != null && loader != null)
            {
                luaEnv.AddLoader(loader.LoadScript);
            }
        }

        public void SetMgr(LuaTable mgrTable)
        {
            MgrTable = mgrTable;
            if (MgrTable != null)
            {
                Action<LuaTable> luaMgrStartAction = mgrTable.Get<Action<LuaTable>>(LuaConst.START_FUNCTION_NAME);
                luaMgrStartAction?.Invoke(mgrTable);

                updateAction = mgrTable.Get<Action<LuaTable, float>>(LuaConst.UPDATE_FUNCTION_NAME);
            }
        }

        public bool IsValid()
        {
            return Env != null && Env.IsValid();
        }

        public void DoUpdate(float deltaTime)
        {
            if (IsValid() && updateAction != null)
            {
                updateAction(MgrTable, deltaTime);
            }
        }

        public void Tick()
        {
            if (IsValid())
            {
                Env.Tick();
            }
        }

        public void GC()
        {
            if (IsValid())
            {
                Env.FullGc();
            }
        }

        public void Dispose()
        {
            updateAction = null;

            MgrTable?.Dispose();
            MgrTable = null;

            Env?.Dispose();
            Env = null;

        }
    }

    public class LuaEnvService : Service,IUpdate
    {
        public const string NAME = "LuaService";

        public const string GLOBAL_MGR_NAME = "EnvMgr";

        public float TickInterval { get; set; } = 0;

        private float elapsedTime = 0.0f;
        private Dictionary<string, LuaEnvData> envDic = new Dictionary<string, LuaEnvData>();

        public LuaEnvService() : base(NAME)
        {
        }

        public override void DoRemove()
        {
            foreach(var kvp in envDic)
            {
                kvp.Value.Dispose();
            }
            envDic.Clear();
        }

        public LuaEnv GetEnv(string envName)
        {
            if(envDic.TryGetValue(envName,out LuaEnvData envData))
            {
                if(envData.IsValid())
                {
                    return envData.Env;
                }
            }
            return null;
        }

        public void DisposeEnv(string envName)
        {
            if (envDic.TryGetValue(envName, out LuaEnvData envData))
            {
                envData.Dispose();

                envDic.Remove(envName);
            }
        }

        public void CreateEnv(string envName,string[] pathFormats,string[] preloadScripts,string mgrScriptPath)
        {
            if(envDic.ContainsKey(envName))
            {
                LogUtil.LogError(LuaConst.LOGGER_NAME, "");
                return;
            }

            LuaEnv luaEnv = new LuaEnv();

            FileScriptLoader scriptLoader = new FileScriptLoader(pathFormats);
            LuaEnvData envData = new LuaEnvData(luaEnv, scriptLoader);
            envDic.Add(envName, envData);

#if DEBUG
            luaEnv.Global.Set(LuaConst.IS_DEBUG_FIELD_NAME, true);
#endif

            if (preloadScripts != null && preloadScripts.Length > 0)
            {
                foreach(var script in preloadScripts)
                {
                    LuaUtility.Require(luaEnv, script);
                }
            }

            if(!string.IsNullOrEmpty(mgrScriptPath))
            {
                LuaTable mgrTable = LuaUtility.Instance(luaEnv, mgrScriptPath);
                if(mgrTable == null)
                {
                    LogUtil.LogError(LuaConst.LOGGER_NAME, "LuaEnvService::CreateEnv->Load mgr Failed.mgrScriptPath = "+mgrScriptPath);
                }else
                {
                    envData.SetMgr(mgrTable);

                    luaEnv.Global.Set(GLOBAL_MGR_NAME, mgrTable);
                }
            }
        }

        public void DoUpdate(float deltaTime)
        {
            bool isNeedTick = false;
            if(TickInterval>0)
            {
                elapsedTime += deltaTime;
                if(elapsedTime>=TickInterval)
                {
                    elapsedTime -= TickInterval;
                    isNeedTick = true;
                }
            }

            foreach (var kvp in envDic)
            {
                kvp.Value.DoUpdate(deltaTime);
                if(isNeedTick)
                {
                    kvp.Value.Tick();
                }
            }
        }

        public void FullGC()
        {
            foreach (var envData in envDic)
            {
                envData.Value.GC();
            }
        }
    }
}
