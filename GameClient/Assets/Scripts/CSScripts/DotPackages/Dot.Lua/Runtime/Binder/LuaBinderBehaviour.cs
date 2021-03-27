using DotEngine.Log;
using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using SystemObject = System.Object;

namespace DotEngine.Lua.Binder
{
    [Serializable]
    public class LuaParamGroup
    {
        public string name;
        public List<LuaParam> paramList = new List<LuaParam>();
    }

    public class LuaBinderBehaviour : MonoBehaviour
    {
        [SerializeField]
        private LuaBinder binder = new LuaBinder();
        [SerializeField]
        private List<LuaParam> registParams = new List<LuaParam>();
        [SerializeField]
        private List<LuaParamGroup> registParamGroups = new List<LuaParamGroup>();

        public LuaTable Table { get; private set; }

        private bool isInited = false;

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
                Table = binder.GetInstance();

                OnInitFinished();
            }
        }

        protected virtual void OnInitFinished()
        {
            SetValue("gameObject", gameObject);
            SetValue("transform", transform);

            foreach(var param in registParams)
            {
                RegistParamToTable(Table, param.name, param);
            }

            foreach(var group in registParamGroups)
            {
                if(group!=null && !string.IsNullOrEmpty(group.name))
                {
                    LuaTable regTable = Env.NewTable();
                    Table.Set(group.name, regTable);
                    for(int i =0;i<group.paramList.Count;++i)
                    {
                        RegistParamToTable(regTable, i + 1, group.paramList[i]);
                    }
                }else
                {
                    LogUtil.Error(LuaUtility.LOGGER_NAME, "");
                }
            }
        }

        private void RegistParamToTable(LuaTable table,string name,LuaParam param)
        {
            if(table!=null && !string.IsNullOrEmpty(name) && param != null)
            {
                SystemObject value = GetValueFromParam(param);
                if (value == null)
                {
                    LogUtil.Error(LuaUtility.LOGGER_NAME, $"LuaBinderBehaviour:RegistParamToTable->the value of the param({name}) is null");
                }
                else
                {
                    table.Set(name, value);
                }
            }
        }

        private void RegistParamToTable(LuaTable table,int index,LuaParam param)
        {
            if(table!=null && param!=null)
            {
                SystemObject value = GetValueFromParam(param);
                if (value == null)
                {
                    LogUtil.Error(LuaUtility.LOGGER_NAME, "");
                }
                else
                {
                    table.Set(index, value);
                }
            }
        }
        
        private SystemObject GetValueFromParam(LuaParam paramValue)
        {
            if(paramValue != null)
            {
                SystemObject value = paramValue.GetValue();
                if(value != null)
                {
                    if(paramValue.paramType == LuaParamType.UObject)
                    {
                        if(value.GetType().IsSubclassOf(typeof(LuaBinderBehaviour)))
                        {
                            LuaBinderBehaviour lbBeh = value as LuaBinderBehaviour;
                            lbBeh.InitBinder();

                            value = lbBeh.Table;
                        }
                    }
                }
                return value;
            }
            return null;
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
            isInited = false;
        }

        public void SetValue<T>(string name, T value)
        {
            if (IsValid())
            {
                Table.Set(name, value);
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
    }
}
