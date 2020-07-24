using DotEngine.Log;
using System;
using XLua;

namespace DotEngine.Lua
{
    public static class LuaUtility
    {
        public static bool Require(LuaEnv luaEnv, string script)
        {
            if (luaEnv == null)
            {
                LogUtil.LogError(LuaConst.LOGGER_NAME, "luaEnv is null");
                return false;
            }
            if (string.IsNullOrEmpty(script))
            {
                LogUtil.LogError(LuaConst.LOGGER_NAME, "script is empty");
                return false;
            }
            string scriptName = GetScriptName(script);
            if (string.IsNullOrEmpty(scriptName))
            {
                LogUtil.LogError(LuaConst.LOGGER_NAME, "scriptName is empty");
                return false;
            }

            if (!luaEnv.Global.ContainsKey(scriptName))
            {
                luaEnv.DoString(string.Format("require (\"{0}\")", script));
            }

            return true;
        }

        public static LuaTable Instance(LuaEnv luaEnv, string script)
        {
            if (!Require(luaEnv, script))
            {
                return null;
            }

            string scriptName = GetScriptName(script);

            LuaTable classTable = luaEnv.Global.Get<LuaTable>(scriptName);

            Func<LuaTable, LuaTable> callFunc = classTable.Get<Func<LuaTable, LuaTable>>(LuaConst.CTOR_FUNCTION_NAME);
            LuaTable table = callFunc?.Invoke(classTable);
            
            classTable.Dispose();

            return table;
        }

        public static string GetScriptName(string script)
        {
            if (string.IsNullOrEmpty(script))
            {
                LogUtil.LogError(LuaConst.LOGGER_NAME, "script is empty");
                return null;
            }

            string scriptName = script;
            int index = script.LastIndexOf("/");
            if (index > 0)
            {
                scriptName = script.Substring(index + 1);
            }
            return scriptName;
        }
    }
}
