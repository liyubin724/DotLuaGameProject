using UnityEngine;
using XLua;
using SystemObject = System.Object;

namespace DotEngine.Lua
{
    public static class LuaUtility
    {
        public const string LOG_TAG = "Lua";

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

        public static LuaTable RequireAndInstanceWith(LuaEnv luaEnv, string scriptPath, params SystemObject[] operateParams)
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

        //------------------------------------------

        public const string LOCAL_REQUIRE_FORMAT = "local table = require('{0}') return table";

        public const string INIT_FUNCTION_NAME = "DoInit";
        public const string AWAKE_FUNCTION_NAME = "DoAwake";
        public const string ENABLE_FUNCTION_NAME = "DoEnable";
        public const string START_FUNCTION_NAME = "DoStart";
        public const string DISABLE_FUNCTION_NAME = "DoDisable";
        public const string UPDATE_FUNCTION_NAME = "DoUpdate";
        public const string LATEUPDATE_FUNCTION_NAME = "DoLateUpdate";
        public const string FIXEDUPDATE_FUNCTION_NAME = "DoFixedUpdate";
        public const string DESTROY_FUNCTION_NAME = "DoDestroy";

        private const string SCRIPT_ASSET_DIR = "Assets/Scripts/LuaScripts/";
        private const string SCRIPT_EXTENSION = ".txt";

        public static string GetScriptFilePathInProject(string scriptPath)
        {
            return $"{Application.dataPath}/Scripts/LuaScripts/{scriptPath}{SCRIPT_EXTENSION}";
        }

        public static string GetScriptPath(string scriptAssetPath)
        {
            if (string.IsNullOrEmpty(scriptAssetPath))
            {
                return null;
            }
            return scriptAssetPath.Replace("\\", "/").Replace(SCRIPT_ASSET_DIR, "").Replace(SCRIPT_EXTENSION, "");
        }


        //----------------------------------
        private const string SCRIPT_DIR = "Scripts/LuaScripts/";

        public static string GetScriptAssetPath(string scriptPath)
        {
            return $"{SCRIPT_ASSET_DIR}{scriptPath}{SCRIPT_EXTENSION}";
        }

        public static string GetScriptDiskPath(string scriptPath)
        {
            return $"{Application.dataPath}/{SCRIPT_DIR}{scriptPath}{SCRIPT_EXTENSION}";
        }


        public static string GetScriptPathFormat()
        {
#if UNITY_EDITOR
            return $"{Application.dataPath}/{SCRIPT_DIR}{{0}}{SCRIPT_EXTENSION}";
#else
           return $"{Application.dataPath}/{SCRIPT_DIR}{{0}}{SCRIPT_EXTENSION}";
#endif
        }
    }
}
