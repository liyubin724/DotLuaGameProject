﻿using DotEngine.Log;
using DotEngine.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Lua
{
    public sealed class LuaEnvManager
    {
        private static readonly string LUA_OOP_PATH = "DotLua/OOP/oop";
        private static readonly string LUA_LAUNCH_PATH = "DotLua/Launcher";

        public LuaEnv Env { get; private set; } = null;

        public LuaTable LaunchTable { get; private set; } = null;
        private Action<float, float> updateAction = null;
        private Action lateUpdateAction = null;
        private Action destroyAction = null;

        public LuaTable OOPTable { get; private set; } = null;
        private Func<string,LuaTable> usingFunc = null;

        private Dictionary<string, LuaTable> cachedClassDic = new Dictionary<string, LuaTable>();

        private LuaEnvBehaviour envBehaviour = null;
        public bool IsValid
        {
            get
            {
                return Env != null && Env.IsValid();
            }
        }

        private static LuaEnvManager manager = null;

        public static LuaEnvManager GetInstance()
        {
            return manager;
        }

        private LuaEnvManager()
        {
            if(manager!=null)
            {
                LogUtil.Error(LuaUtility.LOGGER_NAME, "");
                return;
            }
            manager = this;
            DoInit();
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
            
            OOPTable = Require(LUA_OOP_PATH);
            usingFunc = OOPTable.Get<Func<string,LuaTable>>("using");

            LaunchTable = RequireGlobal(LUA_LAUNCH_PATH);
            updateAction = LaunchTable.Get<Action<float, float>>(LuaUtility.UPDATE_FUNCTION_NAME);
            lateUpdateAction = LaunchTable.Get<Action>(LuaUtility.LATEUPDATE_FUNCTION_NAME);
            destroyAction = LaunchTable.Get<Action>(LuaUtility.DESTROY_FUNCTION_NAME);

            Action startAction = LaunchTable.Get<Action>(LuaUtility.START_FUNCTION_NAME);
            startAction?.Invoke();
        }

        public void Startup()
        {
            envBehaviour = DontDestroyUtility.CreateComponent<LuaEnvBehaviour>();
        }

        public void Shuntdown()
        {
            UnityObject.Destroy(envBehaviour.gameObject);
            envBehaviour = null;
        }

        public void DoUpdate()
        {
            if(IsValid)
            {
                updateAction?.Invoke(Time.deltaTime, Time.unscaledDeltaTime);
                Env.Tick();
            }
        }

        public void DoLateUpdate()
        {
            if(IsValid)
            {
                lateUpdateAction?.Invoke();
            }
        }

        public void DoDestroy()
        {
            if(IsValid)
            {
                destroyAction?.Invoke();

                LaunchTable.Dispose();
                OOPTable.Dispose();

                foreach(var kvp in cachedClassDic)
                {
                    kvp.Value.Dispose();
                }

            }
            cachedClassDic.Clear();
            updateAction = null;
            lateUpdateAction = null;
            destroyAction = null;
            usingFunc = null;

            if(IsValid)
            {
                Env.Dispose();
            }
            Env = null;
        }

        public void FullGC()
        {
            if(IsValid)
            {
                Env.FullGc();
            }
        }

        public LuaTable RequireGlobal(string scriptPath)
        {
            if (string.IsNullOrEmpty(scriptPath) || !IsValid)
            {
                LogUtil.Error(LuaUtility.LOGGER_NAME, "");
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

        public LuaTable Require(string scriptPath)
        {
            if(string.IsNullOrEmpty(scriptPath) || !IsValid)
            {
                LogUtil.Error(LuaUtility.LOGGER_NAME, "");
                return null;
            }

            SystemObject[] values = Env.DoString($"require(\"{scriptPath}\")");
            if (values != null && values.Length > 0)
            {
                return values[0] as LuaTable;
            }
            else
            {
                LogUtil.Error(LuaUtility.LOGGER_NAME, "");
                return null;
            }
        }

        public LuaTable UsingClass(string scriptPath)
        {
            if(!cachedClassDic.TryGetValue(scriptPath,out LuaTable table))
            {
                return table;
            }
            table = usingFunc(scriptPath);
            cachedClassDic.Add(scriptPath, table);

            return table;
        }

        public LuaTable InstanceClass(string scriptPath)
        {
            LuaTable classTable = UsingClass(scriptPath);
            if(classTable !=null)
            {
                Func<LuaTable> newFunc = classTable.Get<Func<LuaTable>>("new");
                if(newFunc!=null)
                {
                    return newFunc();
                }
            }
            return null;
        }

        public LuaTable InstanceClassWith<P>(string scriptPath,P p)
        {
            LuaTable classTable = UsingClass(scriptPath);
            if (classTable != null)
            {
                Func<P, LuaTable> newFunc = classTable.Get<Func<P, LuaTable>>("new");
                if (newFunc != null)
                {
                    return newFunc(p);
                }
            }
            return null;
        }

        public LuaTable InstanceClassWith<P1,P2>(string scriptPath,P1 p1,P2 p2)
        {
            LuaTable classTable = UsingClass(scriptPath);
            if (classTable != null)
            {
                Func<P1, P2, LuaTable> newFunc = classTable.Get<Func<P1, P2, LuaTable>>("new");
                if (newFunc != null)
                {
                    return newFunc(p1, p2);
                }
            }
            return null;
        }

        public LuaTable InstanceClassWith<P1, P2,P3>(string scriptPath, P1 p1, P2 p2,P3 p3)
        {
            LuaTable classTable = UsingClass(scriptPath);
            if (classTable != null)
            {
                Func<P1,P2,P3,LuaTable> newFunc = classTable.Get<Func<P1, P2, P3, LuaTable>>("new");
                if (newFunc != null)
                {
                    return newFunc(p1,p2,p3);
                }
            }
            return null;
        }

        private string GetScriptName(string scriptPath)
        {
            string name = scriptPath;
            int index = name.LastIndexOf("/");
            if(index>0)
            {
                name = name.Substring(index + 1);
            }
            return name;
        }
    }
}
