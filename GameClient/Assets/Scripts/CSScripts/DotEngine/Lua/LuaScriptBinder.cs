﻿using DotEngine.Log;
using System;
using UnityEngine;
using XLua;

namespace DotEngine.Lua
{
    [Serializable]
    public class LuaScriptBinder
    {
        [SerializeField]
        private string m_ScriptFilePath = null;
        [SerializeField]
        private LuaOperateParam[] m_ConstructorParams = new LuaOperateParam[0];
        
        public LuaTable ObjTable { get; private set; }

        private bool m_IsInited = false;

        public LuaScriptBinder()
        { }

        public LuaScriptBinder(string filePath)
        {
            m_ScriptFilePath = filePath;
        }

        public bool IsValid()
        {
            Facade facade = Facade.GetInstance();
            if(facade!=null)
            {
                LuaEnvService service = facade.GetServicer<LuaEnvService>(LuaEnvService.NAME);
                return service != null && service.IsValid() && ObjTable != null;
            }
            return false;
        }

        public bool InitLua()
        {
            if(m_IsInited)
            {
                return IsValid();
            }

            LuaEnvService service = Facade.GetInstance().GetServicer<LuaEnvService>(LuaEnvService.NAME);
            if (!string.IsNullOrEmpty(m_ScriptFilePath))
            {
                ObjTable = service.InstanceScript(m_ScriptFilePath);

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
