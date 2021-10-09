using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using SystemObject = System.Object;

namespace DotEngine.Lua
{
    public class LuaBindBehaviour : MonoBehaviour
    {
        public string bridgerName = null;
        public string scriptPath = null;
        public List<LuaParamValue> ctrParamValues = new List<LuaParamValue>();

        private LuaEnv luaEnv;
        private bool isInited = false;

        public LuaEnv Env
        {
            get
            {
                if (luaEnv == null)
                {
                    LuaManager mgr = LuaManager.GetInstance();
                    if (mgr != null)
                    {
                        LuaBridger bridger = mgr.GetBridger(bridgerName);
                        if (bridger != null)
                        {
                            luaEnv = bridger.Env;
                        }
                    }
                }
                return luaEnv;
            }
        }

        public LuaTable Table { get; private set; }

        public bool IsValid()
        {
            if (isInited && Env.IsValid() && Table != null)
            {
                return true;
            }
            return false;
        }

        public void InitBehaviour()
        {
            if (!isInited)
            {
                isInited = true;

                if (ctrParamValues.Count == 0)
                {
                    Table = LuaUtility.RequireAndInstance(Env, scriptPath);
                }
                else
                {
                    SystemObject[] arr = new SystemObject[ctrParamValues.Count];
                    for (int i = 0; i < ctrParamValues.Count; ++i)
                    {
                        LuaParamValue lpv = ctrParamValues[i];
                        if (lpv != null)
                        {
                            arr[i] = lpv.GetValue();
                        }
                    }

                    Table = LuaUtility.RequireAndInstanceWith(Env, scriptPath, arr);
                }

                OnInitFinished();
            }
        }

        protected virtual void OnInitFinished()
        {
            if (IsValid())
            {
                SetValue("gameObject", gameObject);
                SetValue("transform", transform);
            }
        }

        protected virtual void Awake()
        {
            InitBehaviour();
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

        public void SetValue<V>(string name, V value)
        {
            Table.Set(name, value);
        }

        public void SetValue<K, V>(K key, V value)
        {
            Table.Set(key, value);
        }

        public void CallActionWithParams(string funcName, params LuaParamValue[] values)
        {
            if (values == null || values.Length == 0)
            {
                CallAction(funcName);
            }
            else
            {
                SystemObject[] paramValues = new SystemObject[values.Length + 1];
                paramValues[0] = Table;
                for (int i = 0; i < values.Length; ++i)
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

        public void CallActionWithObjects(string funcName, params SystemObject[] values)
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

    }
}
