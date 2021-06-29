using DotEngine.Log;
using DotEngine.Lua.Binder;
using DotEngine.Lua.Localization;
using DotEngine.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using XLua;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Lua
{
    public class LuaEnvManager
    {
        private static string ROOT_NAME = "LuaEnv-Root";
        private static LuaEnvManager manager = null;

        public static LuaEnvManager GetInstance()
        {
            if(manager == null)
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



        private LuaEnvManager()
        {
            
        }

        public void Startup()
        {
            Env = new LuaEnv();
            Env.AddLoader(GetScriptBytes);

#if DEBUG
            Global.Set("isDebug", true);
#endif

            GameObject gObj = new GameObject(ROOT_NAME);
            updateBehaviour = gObj.AddComponent<LuaUpdateBehaviour>();
            UnityObject.DontDestroyOnLoad(gObj);


        }

        public void Shuntdown()
        {

        }

        private ScriptLoader GetLoader()
        {
            return new FileScriptLoader();
        }

        private byte[] GetScriptBytes(ref string scriptPath)
        {
            scriptPath = $"{Application.dataPath}/Scripts/LuaScripts/{scriptPath}.txt";
            if(File.Exists(scriptPath))
            {
                return File.ReadAllBytes(scriptPath);
            }
            return new byte[0];
        }
        //--------------------------------------------
        private static readonly string LUA_OOP_PATH = "DotLua/OOP/oop";

        private static readonly string LUA_INIT_PATH = "Game/GameInit";
        private static readonly string LUA_GLOBAL_NAME = "Game";
        private static readonly string LUA_GET_LANGUAGE_FUNC_NAME = "GetLanguage";






        public LuaTable GameTable { get; private set; } = null;
        private Action<float, float> updateAction = null;
        private Action lateUpdateAction = null;

        public LuaTable OOPTable { get; private set; } = null;
        private Func<string, LuaTable> usingFunc = null;
        private Func<string, LuaTable> instanceFunc = null;
        private LuaFunction instanceWithFunc = null;

        private LuaUpdateBehaviour envBehaviour = null;

        private LuaLocalLanguage localLanguage = null;
        public LuaLocalLanguage Language
        {
            get
            {
                return localLanguage;
            }
        }



        private void DoInit()
        {
            Env = new LuaEnv();
            ScriptLoader scriptLoader;
#if UNITY_EDITOR
            scriptLoader = new FileScriptLoader();
#else
#endif
            Env.AddLoader(scriptLoader.LoadScript);

#if DEBUG
            Global.Set("isDebug", true);
#endif
            Env.AddBuildin("rapidjson", XLua.LuaDLL.Lua.LoadRapidJson);
            Global.Set("isUsingRapidjson", true);

            Env.AddBuildin("pb", XLua.LuaDLL.Lua.LoadLuaProfobuf);

            OOPTable = RequireAndGetLocalTable(LUA_OOP_PATH);
            usingFunc = OOPTable.Get<Func<string, LuaTable>>("using");
            instanceFunc = OOPTable.Get<Func<string, LuaTable>>("instance");
            instanceWithFunc = OOPTable.Get<LuaFunction>("instancewith");

            Require(LUA_INIT_PATH);
            GameTable = Global.Get<LuaTable>(LUA_GLOBAL_NAME);
            updateAction = GameTable.Get<Action<float, float>>(LuaUtility.UPDATE_FUNCTION_NAME);
            lateUpdateAction = GameTable.Get<Action>(LuaUtility.LATEUPDATE_FUNCTION_NAME);

            Action initAction = GameTable.Get<Action>(LuaUtility.INIT_FUNCTION_NAME);
            initAction?.Invoke();
        }

        public void SetLanguage(LuaTable language)
        {
            if(localLanguage == null)
            {
                localLanguage = new LuaLocalLanguage(language);
            }else
            {
                localLanguage.ChangeLanguage(language);
            }
        }

        public void Startup()
        {
            envBehaviour = DontDestroyUtility.CreateComponent<LuaUpdateBehaviour>();
        }

        public void Shuntdown()
        {
            if(envBehaviour !=null)
            {
                UnityObject.Destroy(envBehaviour.gameObject);
                envBehaviour = null;
            }

            if (IsValid)
            {
                if(localLanguage!=null)
                {
                    localLanguage.Dispose();
                }

                LuaFunction destroyFunc = GameTable.Get<LuaFunction>(LuaUtility.DESTROY_FUNCTION_NAME);
                destroyFunc.Action();
                destroyFunc.Dispose();

                instanceWithFunc.Dispose();
                OOPTable.Dispose();
                GameTable.Dispose();
            }

            localLanguage = null;

            updateAction = null;
            lateUpdateAction = null;
            usingFunc = null;
            instanceFunc = null;
            instanceWithFunc = null;
            GameTable = null;
            OOPTable = null;

            if (IsValid)
            {
                Env.FullGc();
                Env.Dispose();
            }
            Env = null;
            manager = null;
        }

        public void DoStart()
        {
            Action startAction = GameTable.Get<Action>(LuaUtility.START_FUNCTION_NAME);
            startAction?.Invoke();
        }

        public void DoUpdate()
        {
            if (IsValid)
            {
                updateAction?.Invoke(Time.deltaTime, Time.unscaledDeltaTime);
                Env.Tick();
            }
        }

        public void DoLateUpdate()
        {
            if (IsValid)
            {
                lateUpdateAction?.Invoke();
            }
        }

        public void FullGC()
        {
            if (IsValid)
            {
                Env.FullGc();
            }
        }

        public void Require(string scriptPath)
        {
            if (string.IsNullOrEmpty(scriptPath) || !IsValid)
            {
                LogUtil.Error(LuaUtility.LOG_TAG, "");
                return;
            }

            Env.DoString($"require(\"{scriptPath}\")");
        }

        public LuaTable RequireAndGetGlobalTable(string scriptPath)
        {
            if (string.IsNullOrEmpty(scriptPath) || !IsValid)
            {
                LogUtil.Error(LuaUtility.LOG_TAG, "");
                return null;
            }

            string name = GetScriptName(scriptPath);
            LuaTable table = Env.Global.Get<LuaTable>(name);
            if (table == null)
            {
                Env.DoString($"require(\"{scriptPath}\")");
                table = Env.Global.Get<LuaTable>(name);
            }
            return table;
        }

        public LuaTable RequireAndGetLocalTable(string scriptPath)
        {
            if (string.IsNullOrEmpty(scriptPath) || !IsValid)
            {
                LogUtil.Error(LuaUtility.LOG_TAG, "");
                return null;
            }

            SystemObject[] values = Env.DoString(string.Format(LuaUtility.LOCAL_REQUIRE_FORMAT, scriptPath));
            if (values != null && values.Length > 0)
            {
                return values[0] as LuaTable;
            }
            else
            {
                LogUtil.Error(LuaUtility.LOG_TAG, "");
                return null;
            }
        }

        public LuaTable UsingClass(string scriptPath)
        {
            if (string.IsNullOrEmpty(scriptPath))
            {
                LogUtil.Error(LuaUtility.LOG_TAG, "");
                return null;
            }
            return usingFunc(scriptPath);
        }

        public LuaTable InstanceClass(string scriptPath)
        {
            if (string.IsNullOrEmpty(scriptPath))
            {
                LogUtil.Error(LuaUtility.LOG_TAG, "");
                return null;
            }

            return instanceFunc(scriptPath);
        }

        public LuaTable InstanceClassWith(string scriptPath, params LuaParam[] operateParams)
        {
            List<SystemObject> list = new List<SystemObject>();
            list.Add(scriptPath);
            if (operateParams != null && operateParams.Length > 0)
            {
                foreach (var p in operateParams)
                {
                    list.Add(p.GetValue());
                }
            }
            return InstanceClassWith(scriptPath, list.ToArray());
        }

        public LuaTable InstanceClassWith(string scriptPath, params SystemObject[] values)
        {
            return instanceWithFunc.Func<LuaTable>(scriptPath, values);
        }

        private string GetScriptName(string scriptPath)
        {
            string name = scriptPath;
            int index = name.LastIndexOf("/");
            if (index > 0)
            {
                name = name.Substring(index + 1);
            }
            return name;
        }
    }
}
