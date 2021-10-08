using DotEngine.Lua;

namespace Game
{
    public static class GameLauncher
    {
        private static readonly string DEFAULT_LUA_NAME = "Default";
        private static readonly string DEFAULT_LUA_LAUNCHER_SCRIPT_PATH = "Launcher";

        public static void Startup()
        {
            LuaManager luaMgr = LuaManager.GetInstance();
            luaMgr.CreateBridger(DEFAULT_LUA_NAME, DEFAULT_LUA_LAUNCHER_SCRIPT_PATH, true);
        }

        public static void Shutdown()
        {
            LuaManager luaMgr = LuaManager.GetInstance();
            luaMgr.DisposeBridger(DEFAULT_LUA_NAME);
        }
    }
}
