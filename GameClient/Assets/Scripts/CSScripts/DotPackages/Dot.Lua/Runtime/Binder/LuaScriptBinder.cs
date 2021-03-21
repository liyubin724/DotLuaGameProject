using System;
using UnityEngine;
using XLua;

namespace DotEngine.Lua.Binder
{
    [Serializable]
    public class LuaScriptBinder
    {
        [SerializeField]
        private string scriptPath = null;

        public LuaScriptBinder()
        { }

        public LuaScriptBinder(string scriptPath)
        {
            this.scriptPath = scriptPath;
        }

        public LuaTable Instance()
        {
            return LuaEnvManager.GetInstance().InstanceClass(scriptPath);
        }

        public LuaTable InstanceWith(LuaOperateParam[] values)
        {
            return LuaEnvManager.GetInstance().InstanceClassWith(scriptPath, values);
        }
    }
}
