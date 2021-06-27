using System;
using System.Collections.Generic;
using UnityEngine;
using XLua;

namespace DotEngine.Lua.Binder
{
    [Serializable]
    public class LuaBinder
    {
        [SerializeField]
        private string scriptPath = null;
        [SerializeField]
        private List<LuaParam> paramValues = new List<LuaParam>();

        public LuaBinder()
        { }

        public LuaBinder(string scriptPath)
        {
            this.scriptPath = scriptPath;
        }

        public LuaTable GetInstance()
        {
            if(paramValues.Count == 0)
            {
                return LuaEnvManager.GetInstance().InstanceClass(scriptPath);
            }
            else
            {
                return LuaEnvManager.GetInstance().InstanceClassWith(scriptPath, paramValues.ToArray());
            }
        }
    }
}
