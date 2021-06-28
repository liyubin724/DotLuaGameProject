using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using SystemObject = System.Object;

namespace DotEngine.Lua.Binder
{
    public class LuaBinderBehaviour : MonoBehaviour
    {
        public string scriptPath = null;
        public List<LuaParam> paramValues = new List<LuaParam>();

        private LuaEnv luaEnv;
        private bool isInited = false;

        public LuaEnv Env
        {
            get
            {
                if (luaEnv == null)
                {
                    luaEnv = LuaEnvManager.GetInstance().Env;
                }
                return luaEnv;
            }
        }

        public LuaTable Table { get; private set; }

        public bool IsValid()
        {
            if (isInited && Env != null && Table != null)
            {
                return true;
            }

            return false;
        }

        public void InitBinder()
        {
            if (!isInited)
            {
                isInited = true;

                Table = GetInstance();

                OnInitFinished();
            }
        }

        protected virtual void OnInitFinished()
        {
            SetValue("gameObject", gameObject);
            SetValue("transform", transform);
        }

        protected virtual void Awake()
        {
            InitBinder();
            CallAction(LuaUtility.AWAKE_FUNCTION_NAME);
        }

        protected virtual void Start()
        {
            CallAction(LuaUtility.START_FUNCTION_NAME);
        }

        protected virtual void OnEnable()
        {
            CallAction(LuaUtility.ENABLE_FUNCTION_NAME);
        }

        protected virtual void OnDisable()
        {
            CallAction(LuaUtility.DISABLE_FUNCTION_NAME);
        }

        protected virtual void OnDestroy()
        {
            if (IsValid())
            {
                CallAction(LuaUtility.DESTROY_FUNCTION_NAME);

                Table.Dispose();
            }
            Table = null;
            luaEnv = null;
            isInited = false;
        }

        public void SetValue<T>(string name, T value)
        {
            Table.Set(name, value);
        }

        public void CallActionWith(string funcName,params LuaParam[] values)
        {
            if (values == null || values.Length == 0)
            {
                CallAction(funcName);
            }
            else
            {
                SystemObject[] paramValues = new SystemObject[values.Length + 1];
                paramValues[0] = Table;
                for(int i =0;i<values.Length;++i)
                {
                    paramValues[i + 1] = values[i].GetValue();
                }
                LuaFunction func = Table.Get<LuaFunction>(funcName);
                if (func != null)
                {
                    func.ActionParams(paramValues);
                }
                func?.Dispose();
            }
        }    

        public void CallActionWith(string funcName, params SystemObject[] values)
        {
            if (values == null || values.Length == 0)
            {
                CallAction(funcName);
            }
            else
            {
                SystemObject[] paramValues = new SystemObject[values.Length + 1];
                paramValues[0] = Table;
                Array.Copy(values, 0, paramValues, 1, values.Length);
                LuaFunction func = Table.Get<LuaFunction>(funcName);
                if (func != null)
                {
                    func.ActionParams(paramValues);
                }
                func?.Dispose();
            }
        }

        public void CallAction(string funcName)
        {
            if (IsValid())
            {
                Action<LuaTable> action = Table.Get<Action<LuaTable>>(funcName);
                action?.Invoke(Table);
            }
        }

        public void CallAction<T>(string funcName, T value)
        {
            if (IsValid())
            {
                Action<LuaTable, T> action = Table.Get<Action<LuaTable, T>>(funcName);
                action?.Invoke(Table, value);
            }
        }

        public void CallAction<T1, T2>(string funcName, T1 value1, T2 value2)
        {
            if (IsValid())
            {
                Action<LuaTable, T1, T2> action = Table.Get<Action<LuaTable, T1, T2>>(funcName);
                action?.Invoke(Table, value1, value2);
            }
        }

        public R CallFunc<R>(string funcName)
        {
            if (IsValid())
            {
                Func<LuaTable, R> func = Table.Get<Func<LuaTable, R>>(funcName);
                return func(Table);
            }
            return default(R);
        }

        public R CallFunc<T, R>(string funcName, T value)
        {
            if (IsValid())
            {
                Func<LuaTable, T, R> func = Table.Get<Func<LuaTable, T, R>>(funcName);
                return func(Table, value);
            }
            return default(R);
        }

        public R CallFunc<T1, T2, R>(string funcName, T1 value1, T2 value2)
        {
            if (IsValid())
            {
                Func<LuaTable, T1, T2, R> func = Table.Get<Func<LuaTable, T1, T2, R>>(funcName);
                return func(Table, value1, value2);
            }
            return default(R);
        }

        public LuaTable GetInstance()
        {
            if (paramValues.Count == 0)
            {
                return LuaUtility.RequireAndInstance(luaEnv, scriptPath);
            }
            else
            {
                SystemObject[] values = new SystemObject[paramValues.Count];
                for (int i = 0; i < paramValues.Count; ++i)
                {
                    values[i] = paramValues[i].GetValue();
                }
                return LuaUtility.RequireAndInstanceWith(luaEnv, scriptPath, values);
            }
        }
    }
}
