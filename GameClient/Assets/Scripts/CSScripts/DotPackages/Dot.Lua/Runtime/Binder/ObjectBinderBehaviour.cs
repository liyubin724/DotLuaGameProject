using DotEngine.Log;
using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using SystemObject = System.Object;

namespace DotEngine.Lua.Binder
{
    public class ObjectBinderBehaviour : ScriptBinderBehaviour
    {
        [Serializable]
        public class LuaOperateParamArray
        {
            public string name;
            public List<LuaOperateParam> operateParams = new List<LuaOperateParam>();
        }

        [SerializeField]
        private List<LuaOperateParam> m_RegistObjects = new List<LuaOperateParam>();
        [SerializeField]
        private List<LuaOperateParamArray> m_RegistArrayObjects = new List<LuaOperateParamArray>();

        protected override void OnInitFinished()
        {
            if (IsValid())
            {
                RegistObjectsToTable();
                RegistObjectArrayToTable();
            }
        }

        private void RegistObjectsToTable()
        {
            if (m_RegistObjects != null && m_RegistObjects.Count > 0)
            {
                for (int i = 0; i < m_RegistObjects.Count; i++)
                {
                    LuaOperateParam operateParam = m_RegistObjects[i];
                    if (string.IsNullOrEmpty(operateParam.name))
                    {
                        LogUtil.Error(LuaUtility.LOGGER_NAME, "ObjectBinderBehaviour::OnInitFinished->the name of param is empty");
                        continue;
                    }

                    SystemObject regObject = GetRegistObject(operateParam);
                    if (regObject == null)
                    {
                        LogUtil.Error(LuaUtility.LOGGER_NAME, "ObjectBinderBehaviour::OnInitFinished->the registerObject is null");
                        continue;
                    }
                    Table.Set(m_RegistObjects[i].name, regObject);
                }
            }
        }

        private void RegistObjectArrayToTable()
        {
            for (int i = 0; i < m_RegistArrayObjects.Count; ++i)
            {
                LuaOperateParamArray operateParamArray = m_RegistArrayObjects[i];
                if (string.IsNullOrEmpty(operateParamArray.name))
                {
                    LogUtil.Error(LuaUtility.LOGGER_NAME, "ObjectBinderBehaviour::RegistObjectArrayToTable->the name is empty");
                    continue;
                }

                LuaTable regTable = Env.NewTable();

                for (int j = 0; j < operateParamArray.operateParams.Count; ++j)
                {
                    regTable.Set(j+1, GetRegistObject(operateParamArray.operateParams[j]));
                }

                Table.Set(operateParamArray.name, regTable);
                regTable.Dispose();
            }
        }

        private SystemObject GetRegistObject(LuaOperateParam operateParam)
        {
            if (operateParam != null)
            {
                SystemObject regObject = operateParam.GetValue();
                if (regObject == null)
                {
                    LogUtil.Error(LuaUtility.LOGGER_NAME, "ObjectBinderBehaviour::OnInitFinished->the registerObject is null");
                }
                else
                {
                    if (operateParam.paramType == LuaOperateParamType.UObject)
                    {
                        if (regObject.GetType().IsSubclassOf(typeof(ScriptBinderBehaviour)))
                        {
                            ScriptBinderBehaviour binderBehaviour = (ScriptBinderBehaviour)regObject;
                            binderBehaviour.InitBinder();
                            regObject = binderBehaviour.Table;
                        }
                    }
                }

                return regObject;
            }

            return null;
        }
    }
}
