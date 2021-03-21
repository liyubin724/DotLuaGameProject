using System;
using UnityEngine;
using XLua;
using SystemObject = System.Object;

namespace DotEngine.Lua.Binder
{
    public class ScriptBinderBehaviour : MonoBehaviour
    {
        public LuaScriptBinder bindScript = new LuaScriptBinder();
        public LuaOperateParam[] constructorParams = new LuaOperateParam[0];

        private bool m_IsInited = false;
        private LuaTable m_LuaTable = null;
        public LuaTable Table
        {
            get => m_LuaTable;
        }

        public LuaEnv Env
        {
            get
            {
                LuaEnvManager mgr = LuaEnvManager.GetInstance();
                if (mgr != null && mgr.IsValid)
                {
                    return mgr.Env;
                }
                return null;
            }
        }

        public bool IsValid()
        {
            if (m_IsInited && Env != null && Table != null)
            {
                return true;
            }

            return false;
        }

        public void InitBinder()
        {
            if (!m_IsInited)
            {
                m_IsInited = true;
                m_LuaTable = bindScript.InstanceWith(constructorParams);

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

                m_LuaTable.Dispose();
                m_LuaTable = null;
                m_IsInited = false;
            }
        }

        public void SetValue<T>(string name, T value)
        {
            if (IsValid())
            {
                m_LuaTable.Set(name, value);
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
                paramValues[0] = m_LuaTable;
                Array.Copy(values, 0, paramValues, 1, values.Length);
                LuaFunction func = m_LuaTable.Get<LuaFunction>(funcName);
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
                Action<LuaTable> action = m_LuaTable.Get<Action<LuaTable>>(funcName);
                action?.Invoke(m_LuaTable);
            }
        }

        public void CallAction<T>(string funcName, T value)
        {
            if (IsValid())
            {
                Action<LuaTable, T> action = m_LuaTable.Get<Action<LuaTable, T>>(funcName);
                action?.Invoke(m_LuaTable, value);
            }
        }

        public void CallAction<T1, T2>(string funcName, T1 value1, T2 value2)
        {
            if (IsValid())
            {
                Action<LuaTable, T1, T2> action = m_LuaTable.Get<Action<LuaTable, T1, T2>>(funcName);
                action?.Invoke(m_LuaTable, value1, value2);
            }
        }

        public R CallFunc<R>(string funcName)
        {
            if (IsValid())
            {
                Func<LuaTable, R> func = m_LuaTable.Get<Func<LuaTable, R>>(funcName);
                return func(m_LuaTable);
            }
            return default(R);
        }

        public R CallFunc<T, R>(string funcName, T value)
        {
            if (IsValid())
            {
                Func<LuaTable, T, R> func = m_LuaTable.Get<Func<LuaTable, T, R>>(funcName);
                return func(m_LuaTable, value);
            }
            return default(R);
        }

        public R CallFunc<T1, T2, R>(string funcName, T1 value1, T2 value2)
        {
            if (IsValid())
            {
                Func<LuaTable, T1, T2, R> func = m_LuaTable.Get<Func<LuaTable, T1, T2, R>>(funcName);
                return func(m_LuaTable, value1, value2);
            }
            return default(R);
        }
    }
}
