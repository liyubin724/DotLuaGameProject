using DotEngine.Lua.Register;
using System;
using UnityEngine;

namespace DotEngine.Lua.UI
{
    [Serializable]
    public class EventHandlerData
    {
        [SerializeField]
        private ScriptBindBehaviour m_BindBehaviour = null;
        [SerializeField]
        private string m_FuncName = string.Empty;

        public void Invoke()
        {
            if (m_BindBehaviour != null && !string.IsNullOrEmpty(m_FuncName))
            {
                m_BindBehaviour.CallAction(m_FuncName);
            }
        }

        public void InvokeWithParam1<T>(T value)
        {
            if (m_BindBehaviour != null && !string.IsNullOrEmpty(m_FuncName))
            {
                m_BindBehaviour.CallAction<T>(m_FuncName, value);
            }
        }
    }
}
