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

        public LuaScriptBinder()
        { }

        public LuaScriptBinder(string filePath)
        {
            m_ScriptFilePath = filePath;
        }

        public LuaTable InstanceScript()
        {
            LuaEnvService service = Facade.GetInstance().GetServicer<LuaEnvService>(LuaEnvService.NAME);
            if (!string.IsNullOrEmpty(m_ScriptFilePath))
            {
                return service.InstanceScript(m_ScriptFilePath);
            }
            return null;
        }

        public LuaTable InstanceScriptWith(LuaOperateParam[] values)
        {
            LuaEnvService service = Facade.GetInstance().GetServicer<LuaEnvService>(LuaEnvService.NAME);
            if (!string.IsNullOrEmpty(m_ScriptFilePath))
            {
                return service.InstanceScriptWith(m_ScriptFilePath, values);
            }
            return null;
        }
    }
}
