using System;
using UnityEngine;
using XLua;

namespace DotEngine.Lua.Register
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
                Facade facade = Facade.GetInstance();
                if (facade != null)
                {
                    LuaEnvService service = facade.GetServicer<LuaEnvService>(LuaEnvService.NAME);
                    if(service!=null && service.IsValid())
                    {
                        return service.Env;
                    }
                }
                return null;
            }
        }

        public bool IsValid()
        {
            if (m_IsInited && Env!=null && Table!=null)
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
                m_LuaTable = bindScript.InstanceScriptWith(constructorParams);

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
            CallAction(LuaConst.AWAKE_FUNCTION_NAME);
        }

        protected virtual void Start()
        {
            CallAction(LuaConst.START_FUNCTION_NAME);
        }

        protected virtual void OnEnable()
        {
            CallAction(LuaConst.ENABLE_FUNCTION_NAME);
        }

        protected virtual void OnDisable()
        {
            CallAction(LuaConst.DISABLE_FUNCTION_NAME);
        }

        protected virtual void OnDestroy()
        {
            if (IsValid())
            {
                CallAction(LuaConst.DESTROY_FUNCTION_NAME);
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
