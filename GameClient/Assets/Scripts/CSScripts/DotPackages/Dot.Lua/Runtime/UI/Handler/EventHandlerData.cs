using DotEngine.Lua.Register;
using System;
using UnityEngine;
using SystemObject = System.Object;

namespace DotEngine.Lua.UI.Handler
{
    [Serializable]
    public class EventHandlerData
    {
        [SerializeField]
        private ScriptBinderBehaviour m_BindBehaviour = null;
        [SerializeField]
        private string m_FuncName = string.Empty;
        [SerializeField]
        private LuaOperateParam[] m_OperateParams = new LuaOperateParam[0];

        public void Invoke()
        {
            if (m_BindBehaviour != null && !string.IsNullOrEmpty(m_FuncName))
            {
                SystemObject[] values = new SystemObject[m_OperateParams.Length];
                for(int i =0;i<m_OperateParams.Length;++i)
                {
                        values[i] = m_OperateParams[i].GetValue();
                }
                m_BindBehaviour.CallActionWithParams(m_FuncName,values);
            }
        }

        public void InvokeWithParam1<T>(T value)
        {
            if (m_BindBehaviour != null && !string.IsNullOrEmpty(m_FuncName))
            {
                SystemObject[] values = new SystemObject[m_OperateParams.Length+1];
                values[0] = value;
                for (int i = 0; i < m_OperateParams.Length; ++i)
                {
                    values[i+1] = m_OperateParams[i].GetValue();
                }
                m_BindBehaviour.CallActionWithParams(m_FuncName, values);
            }
        }
    }
}
