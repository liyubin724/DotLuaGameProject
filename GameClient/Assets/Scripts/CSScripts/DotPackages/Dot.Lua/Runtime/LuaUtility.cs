using UnityEngine;

namespace DotEngine.Lua
{
    public static class LuaUtility
    {
        public const string LOGGER_NAME = "Lua";

        public const string AWAKE_FUNCTION_NAME = "DoAwake";
        public const string ENABLE_FUNCTION_NAME = "DoEnable";
        public const string START_FUNCTION_NAME = "DoStart";
        public const string DISABLE_FUNCTION_NAME = "DoDisable";
        public const string UPDATE_FUNCTION_NAME = "DoUpdate";
        public const string LATEUPDATE_FUNCTION_NAME = "DoLateUpdate";
        public const string FIXEDUPDATE_FUNCTION_NAME = "DoFixedUpdate";
        public const string DESTROY_FUNCTION_NAME = "DoDestroy";

        private const string SCRIPT_EXTENSION = ".txt";

        public static string GetScriptFilePathInProject(string scriptPath)
        {
            return $"{Application.dataPath}/Scripts/LuaScripts/{scriptPath}{SCRIPT_EXTENSION}";
        }

        //----------------------------------
        private const string SCRIPT_ASSET_DIR = "Assets/Scripts/LuaScripts/";
        private const string SCRIPT_DIR = "Scripts/LuaScripts/";

        public static string GetScriptAssetPath(string scriptPath)
        {
            return $"{SCRIPT_ASSET_DIR}{scriptPath}{SCRIPT_EXTENSION}";
        }

        public static string GetScriptDiskPath(string scriptPath)
        {
            return $"{Application.dataPath}/{SCRIPT_DIR}{scriptPath}{SCRIPT_EXTENSION}";
        }

        public static string GetScriptPath(string scriptAssetPath)
        {
            if(string.IsNullOrEmpty(scriptAssetPath))
            {
                return null;
            }
            return scriptAssetPath.Replace("\\","/").Replace(SCRIPT_ASSET_DIR, "").Replace(SCRIPT_EXTENSION,"");
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
