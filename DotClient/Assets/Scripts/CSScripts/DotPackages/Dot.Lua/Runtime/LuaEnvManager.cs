﻿using System;
using UnityEngine;
using XLua;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Lua
{
    public class LuaEnvManager
    {
        private static string ROOT_NAME = "LuaEnv-Root";

        private static LuaEnvManager manager = null;

        private static readonly string LUA_INIT_PATH = "Game/GameMain";

        public static LuaEnvManager GetInstance()
        {
            if (manager == null)
            {
                manager = new LuaEnvManager();
            }
            return manager;
        }

        private LuaUpdateBehaviour updateBehaviour;

        public LuaEnv Env { get; private set; } = null;
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
        public bool IsValid => Env != null && Env.IsValid();

        private LuaTable gameMainTable;
        private Action<float, float> updateHandler;
        private Action<float, float> lateUpdateHandler;
        private Action<float, float> fixedUpdateHandler;

        public bool IsRunning { get; private set; } = false;

        private LuaEnvManager()
        {

        }

        public void Startup()
        {
            if (IsRunning)
            {
                return;
            }
            IsRunning = true;

            Env = new LuaEnv();
            Env.AddLoader(ScriptLoader.LoadScriptFromProject);

#if DEBUG
            Global.Set("isDebug", true);
#endif

            Env.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
            Global.Set("isRapidJson", true);

            Env.AddBuildin("pb", XLua.LuaDLL.Lua.LoadLuaProfobuf);

            GameObject gObj = new GameObject(ROOT_NAME);
            updateBehaviour = gObj.AddComponent<LuaUpdateBehaviour>();
            UnityObject.DontDestroyOnLoad(gObj);

            gameMainTable = LuaUtility.RequireAndGet(Env, LUA_INIT_PATH);

            Action startAction = gameMainTable.Get<Action>(LuaUtility.START_FUNCTION_NAME);
            startAction?.Invoke();

            updateHandler = gameMainTable.Get<Action<float, float>>(LuaUtility.UPDATE_FUNCTION_NAME);
            lateUpdateHandler = gameMainTable.Get<Action<float, float>>(LuaUtility.UPDATE_FUNCTION_NAME);
            fixedUpdateHandler = gameMainTable.Get<Action<float, float>>(LuaUtility.FIXEDUPDATE_FUNCTION_NAME);
        }

        public void Shuntdown()
        {
            if (!IsRunning)
            {
                return;
            }
            IsRunning = false;

            updateBehaviour = null;
            updateHandler = null;
            lateUpdateHandler = null;
            fixedUpdateHandler = null;

            if (IsValid)
            {
                Action destroyAction = gameMainTable.Get<Action>(LuaUtility.DESTROY_FUNCTION_NAME);
                destroyAction?.Invoke();

                gameMainTable.Dispose();
                gameMainTable = null;

                FullGC();

                Env.Dispose();
                Env = null;
            }

            UnityObject.Destroy(updateBehaviour.gameObject);
            updateBehaviour = null;
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

        public void FullGC()
        {
            if (IsValid)
            {
                GC.Collect();
                Resources.UnloadUnusedAssets();
                GC.Collect();
                Resources.UnloadUnusedAssets();
                GC.Collect();

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

        private LuaLocalization localization = new LuaLocalization();
        public event Action OnLanguageChanged = null;
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
            if(IsRunning)
            {
                Shuntdown();
            }
            manager = null;
        }
    }
}
