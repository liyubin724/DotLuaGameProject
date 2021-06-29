using XLua;
using SystemObject = System.Object;

namespace DotEngine.Lua
{
    public static class LuaUtility
    {
        public const string LOG_TAG = "Lua";

        public const string AWAKE_FUNCTION_NAME = "DoAwake";
        public const string ENABLE_FUNCTION_NAME = "DoEnable";
        public const string START_FUNCTION_NAME = "DoStart";
        public const string DISABLE_FUNCTION_NAME = "DoDisable";
        public const string DESTROY_FUNCTION_NAME = "DoDestroy";

        public const string UPDATE_FUNCTION_NAME = "DoUpdate";
        public const string LATEUPDATE_FUNCTION_NAME = "DoLateUpdate";
        public const string FIXEDUPDATE_FUNCTION_NAME = "DoFixedUpdate";

        public static bool Require(LuaEnv luaEnv, string scriptPath)
        {
            if (string.IsNullOrEmpty(scriptPath))
            {
                Logger.Error(LOG_TAG, "the path of script is empty");
                return false;
            }
            if (luaEnv == null || !luaEnv.IsValid())
            {
                Logger.Error(LOG_TAG, "the env of lua is invalid");
                return false;
            }

            luaEnv.DoString($"require('{scriptPath}')");
            return true;
        }

        public static LuaTable RequireAndGet(LuaEnv luaEnv, string scriptPath)
        {
            if (string.IsNullOrEmpty(scriptPath))
            {
                Logger.Error(LOG_TAG, "the path of script is empty");
                return null;
            }
            if (luaEnv == null || !luaEnv.IsValid())
            {
                Logger.Error(LOG_TAG, "the env of lua is invalid");
                return null;
            }

            string name = GetScriptName(scriptPath);
            LuaTable table = luaEnv.Global.Get<LuaTable>(name);
            if (table == null)
            {
                luaEnv.DoString($"require(\"{scriptPath}\")");
                table = luaEnv.Global.Get<LuaTable>(name);
            }
            return table;
        }

        public static LuaTable RequireAndInstance(LuaEnv luaEnv, string scriptPath)
        {
            if (string.IsNullOrEmpty(scriptPath))
            {
                Logger.Error(LOG_TAG, "the path of script is empty");
                return null;
            }
            if (luaEnv == null || !luaEnv.IsValid())
            {
                Logger.Error(LOG_TAG, "the env of lua is invalid");
                return null;
            }

            SystemObject[] values = luaEnv.DoString($"local data = require('{scriptPath}') return data()");
            if (values != null && values.Length > 0)
            {
                return values[0] as LuaTable;
            }
            else
            {
                Logger.Error(LOG_TAG, "the value is null");
                return null;
            }
        }

        public static LuaTable RequireAndInstanceWith(LuaEnv luaEnv, string scriptPath, SystemObject[] operateParams)
        {
            if (string.IsNullOrEmpty(scriptPath))
            {
                Logger.Error(LOG_TAG, "the path of script is empty");
                return null;
            }
            if (luaEnv == null || !luaEnv.IsValid())
            {
                Logger.Error(LOG_TAG, "the env of lua is invalid");
                return null;
            }
            SystemObject[] values = luaEnv.DoString($"return require('{scriptPath}')");
            if (values != null && values.Length > 0)
            {
                LuaTable classTable = values[0] as LuaTable;
                if (classTable != null)
                {
                    LuaFunction ctrFunc = classTable.Get<LuaFunction>("__call");
                    if (ctrFunc != null)
                    {
                        return ctrFunc.Func<LuaTable>(operateParams);
                    }
                }
            }

            Logger.Error(LOG_TAG, "the value is null");
            return null;
        }

        private static string GetScriptName(string scriptPath)
        {
            string name = scriptPath;
            int index = name.LastIndexOf("/");
            if (index > 0)
            {
                name = name.Substring(index + 1);
            }
            return name;
        }

    }
}
