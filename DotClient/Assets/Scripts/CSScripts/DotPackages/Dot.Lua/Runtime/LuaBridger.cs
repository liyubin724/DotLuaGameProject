using DotEngine.Core;
using DotEngine.Core.Update;
using System;
using UnityEngine;
using XLua;

namespace DotEngine.Lua
{
    public class LuaBridger : IDisposable, IUpdate,ILateUpdate,IFixedUpdate
    {
        private string initScriptPath;
        public LuaEnv Env { get; private set; } = null;
        public bool IsValid => Env != null && Env.IsValid();

        public LuaTable Global
        {
            get
            {
                if (IsValid)
                {
                    return Env.Global;
                }
                return null;
            }
        }

        private LuaTable gameTable;
        private Action<float, float> updateHandler;
        private Action<float, float> lateUpdateHandler;
        private Action<float, float> fixedUpdateHandler;

        public bool IsRunning { get; private set; } = false;

        private LuaLocalization localization = new LuaLocalization();
        public event Action OnLanguageChanged = null;

        public LuaBridger(string initScriptPath)
        {
            this.initScriptPath = initScriptPath;
        }

        public void Startup()
        {
            if (IsRunning)
            {
                return;
            }
            IsRunning = true;

            Env = new LuaEnv();
            Env.AddLoader(LuaScriptLoader.LoadScriptFromProject);

#if DEBUG
            Global.Set("isDebug", true);
#endif

            Env.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
            Global.Set("isRapidJson", true);

            Env.AddBuildin("pb", XLua.LuaDLL.Lua.LoadLuaProfobuf);

            UpdateManager.GetInstance().AddUpdater(this);
            UpdateManager.GetInstance().AddLateUpdater(this);
            UpdateManager.GetInstance().AddFixedUpdater(this);

            gameTable = LuaUtility.RequireAndGet(Env, initScriptPath);
            updateHandler = gameTable.Get<Action<float, float>>(LuaUtility.UPDATE_FUNCTION_NAME);
            lateUpdateHandler = gameTable.Get<Action<float, float>>(LuaUtility.UPDATE_FUNCTION_NAME);
            fixedUpdateHandler = gameTable.Get<Action<float, float>>(LuaUtility.FIXEDUPDATE_FUNCTION_NAME);

            Action startAction = gameTable.Get<Action>(LuaUtility.START_FUNCTION_NAME);
            startAction?.Invoke();
        }

        public void Shuntdown()
        {
            if (!IsRunning)
            {
                return;
            }
            IsRunning = false;

            UpdateManager.GetInstance().RemoveUpdater(this);
            UpdateManager.GetInstance().RemoveLateUpdater(this);
            UpdateManager.GetInstance().RemoveFixedUpdater(this);

            updateHandler = null;
            lateUpdateHandler = null;
            fixedUpdateHandler = null;

            if (IsValid)
            {
                LuaFunction destroyFunc = gameTable.Get<LuaFunction>(LuaUtility.DESTROY_FUNCTION_NAME);
                destroyFunc?.Action();
                destroyFunc?.Dispose();

                gameTable.Dispose();
                gameTable = null;

                FullGC();

                localization?.Dispose();

                Env.Dispose();
                Env = null;
            }
        }

        public void DoUpdate(float deltaTime, float unscaleDeltaTime)
        {
            if (IsValid)
            {
                updateHandler?.Invoke(deltaTime, unscaleDeltaTime);
                Env.Tick();
            }
        }

        public void DoLateUpdate(float deltaTime, float unscaleDeltaTime)
        {
            if (IsValid)
            {
                lateUpdateHandler?.Invoke(deltaTime, unscaleDeltaTime);
            }
        }

        public void DoFixedUpdate(float deltaTime, float unscaleDeltaTime)
        {
            if (IsValid)
            {
                fixedUpdateHandler?.Invoke(deltaTime, unscaleDeltaTime);
            }
        }

        public void DeepGC()
        {
            if (IsValid)
            {
                GC.Collect();
                Resources.UnloadUnusedAssets();
                GC.Collect();
                Resources.UnloadUnusedAssets();
                GC.Collect();
                GC.Collect();

                Env.FullGc();
            }
        }

        public void FullGC()
        {
            if (IsValid)
            {
                Env.FullGc();
            }
        }

        public void StopGC()
        {
            if (IsValid)
            {
                Env.StopGc();
            }
        }

        public void RestartGC()
        {
            if (IsValid)
            {
                Env.RestartGc();
            }
        }

        public float GetUsedMemory()
        {
            if (IsValid)
            {
                return Env.GetTotalMemory();
            }
            return 0.0f;
        }

        
        public void SetLocalization(LuaTable languageTable)
        {
            localization.ChangeLanguage(languageTable);
            OnLanguageChanged?.Invoke();
        }

        public string GetLocalizationText(string locName)
        {
            return localization.GetText(locName);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
