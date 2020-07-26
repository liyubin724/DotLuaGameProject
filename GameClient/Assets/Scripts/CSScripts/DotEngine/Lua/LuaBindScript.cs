using DotEngine.Log;
using System;
using UnityEngine;
using XLua;

namespace DotEngine.Lua
{
    [Serializable]
    public class LuaBindScript
    {
        [SerializeField]
        private string m_ScriptFilePath = null;

        public LuaTable ObjTable { get; private set; }

        private bool m_IsInited = false;

        public LuaBindScript()
        { }

        public LuaBindScript(string filePath)
        {
            m_ScriptFilePath = filePath;
        }

        public bool IsValid()
        {
            LuaEnvService service = Facade.GetInstance().GetService<LuaEnvService>(LuaEnvService.NAME);
            return service.IsValid() && ObjTable != null;
        }

        public bool InitLua()
        {
            if(m_IsInited)
            {
                return IsValid();
            }

            LuaEnvService service = Facade.GetInstance().GetService<LuaEnvService>(LuaEnvService.NAME);
            if (!string.IsNullOrEmpty(m_ScriptFilePath))
            {
                ObjTable = LuaUtility.Instance(service.Env, m_ScriptFilePath);

                if (ObjTable != null)
                {
                    m_IsInited = true;
                    
                    return true;
                }
                else
                {
                    LogUtil.LogError(LuaConst.LOGGER_NAME, $"ScriptBindBehaviour::InitLua->objTable is null.");
                }
            }

            return false;
        }

        public void Dispose()
        {
            ObjTable?.Dispose();
            ObjTable = null;
            m_IsInited = false;
        }

        public void SetValue<T>(string name,T value)
        {
            if(IsValid())
            {
                ObjTable.Set(name, value);
            }
        }

        public void CallAction(string funcName)
        {
            if (ObjTable != null)
            {
                Action<LuaTable> action = ObjTable.Get<Action<LuaTable>>(funcName);
                if(action!=null)
                {
                    action(ObjTable);
                }
            }
        }

        public void CallAction<T>(string funcName,T value)
        {
            if (ObjTable != null)
            {
                Action<LuaTable, T> action = ObjTable.Get<Action<LuaTable, T>>(funcName);
                if(action!=null)
                {
                    action(ObjTable, value);
                }
            }
        }

        public void CallAction<T1,T2>(string funcName,T1 value1,T2 value2)
        {
            if(ObjTable!=null)
            {
                Action<LuaTable, T1, T2> action = ObjTable.Get<Action<LuaTable, T1, T2>>(funcName);
                if(action!=null)
                {
                    action(ObjTable, value1, value2);
                }
            }
        }

        public R CallFunc<R>(string funcName)
        {
            if (ObjTable != null)
            {
                Func<LuaTable, R> func = ObjTable.Get<Func<LuaTable, R>>(funcName);
                if (func != null)
                {
                    return func(ObjTable);
                }
            }
            return default(R);
        }

        public R CallFunc<T,R>(string funcName,T value)
        {
            if (ObjTable != null)
            {
                Func<LuaTable, T,R> func = ObjTable.Get<Func<LuaTable, T,R>>(funcName);
                if (func != null)
                {
                    return func(ObjTable, value);
                }
            }
            return default(R);
        }

        public R CallFunc<T1,T2, R>(string funcName, T1 value1,T2 value2)
        {
            if (ObjTable != null)
            {
                Func<LuaTable, T1,T2, R> func = ObjTable.Get<Func<LuaTable, T1,T2, R>>(funcName);
                if (func != null)
                {
                    return func(ObjTable, value1,value2);
                }
            }
            return default(R);
        }
    }
}
