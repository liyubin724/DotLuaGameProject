using DotEngine.Core;
using System;
using UnityEngine;
using XLua;

namespace DotEngine.Lua
{
    public class LuaBridger : ADispose
    {
        private string initScriptPath;
        public LuaEnv Env { get; private set; } = null;
        public bool IsValid => Env != null && Env.IsValid();
        public LuaTable Global => IsValid ? Env.Global : null;

        private LuaTable gameTable;
        private Action<float, float> updateHandler;
        private Action<float, float> lateUpdateHandler;
        private Action<float, float> fixedUpdateHandler;

        private LuaLocalization localization = new LuaLocalization();
        public event Action OnLanguageChanged = null;

        public LuaBridger(string initScriptPath)
        {
            this.initScriptPath = initScriptPath;
        }

        public void DoStart()
        {
            if (IsValid)
            {
                Debug.LogError("The bridge has been startup");
                return;
            }
            Env = new LuaEnv();
            Env.AddLoader(LuaScriptLoader.LoadScriptFromProject);
#if DEBUG
            Global.Set("isDebug", true);
#endif
            Env.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
            Global.Set("isRapidJson", true);
            Env.AddBuildin("pb", XLua.LuaDLL.Lua.LoadLuaProfobuf);

            gameTable = LuaUtility.RequireAndGet(Env, initScriptPath);
            updateHandler = gameTable.Get<Action<float, float>>(LuaUtility.UPDATE_FUNCTION_NAME);
            lateUpdateHandler = gameTable.Get<Action<float, float>>(LuaUtility.UPDATE_FUNCTION_NAME);
            fixedUpdateHandler = gameTable.Get<Action<float, float>>(LuaUtility.FIXEDUPDATE_FUNCTION_NAME);

            Action startAction = gameTable.Get<Action>(LuaUtility.START_FUNCTION_NAME);
            startAction?.Invoke();
        }

        public void DoDestroy()
        {
            if (!IsValid)
            {
                Debug.LogError("The bridge has been disposed");
                return;
            }

            LuaFunction destroyFunc = gameTable.Get<LuaFunction>(LuaUtility.DESTROY_FUNCTION_NAME);
            destroyFunc?.Action();
            destroyFunc?.Dispose();

            Dispose();
        }

        #region Update

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
        #endregion

        #region GC
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
        #endregion

        public float GetUsedMemory()
        {
            if (IsValid)
            {
                return Env.GetTotalMemory();
            }
            return 0.0f;
        }

        #region Localization
        public void SetLocalization(LuaTable languageTable)
        {
            localization.ChangeLanguage(languageTable);
            OnLanguageChanged?.Invoke();
        }

        public string GetLocalizationText(string locName)
        {
            return localization.GetText(locName);
        }
        #endregion

        #region Dispose
        protected override void DisposeManagedResource()
        {
            updateHandler = null;
            lateUpdateHandler = null;
            fixedUpdateHandler = null;
        }

        protected override void DisposeUnmanagedResource()
        {
            if (!IsValid)
            {
                return;
            }

            localization?.Dispose();
            gameTable?.Dispose();
            Env.Dispose();

            OnLanguageChanged = null;
            localization = null;
            gameTable = null;
            Env = null;
        }
        #endregion
    }
}
