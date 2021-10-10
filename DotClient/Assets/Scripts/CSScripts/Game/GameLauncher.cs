using DotEngine.Core.Update;
using DotEngine.Log;
using DotEngine.Lua;
using DotEngine.UPool;

namespace Game
{
    public static class GameLauncher
    {
        private static readonly string DEFAULT_LUA_NAME = "Default";
        private static readonly string DEFAULT_LUA_LAUNCHER_SCRIPT_PATH = "Launcher";

        public static void Startup()
        {
            LogUtil.Init();
            LogUtil.AddAppender(new UnityConsoleAppender());

            UpdateManager.InitMgr();

            PoolManager.InitMgr();

            LuaManager luaMgr = LuaManager.InitMgr();
            luaMgr.CreateBridger(DEFAULT_LUA_NAME, DEFAULT_LUA_LAUNCHER_SCRIPT_PATH, true);
        }

        public static void Shutdown()
        {
            LuaManager luaMgr = LuaManager.GetInstance();
            luaMgr.DisposeBridger(DEFAULT_LUA_NAME);
            LuaManager.DisposeMgr();

            PoolManager.DisposeMgr();

            UpdateManager.DisposeMgr();

            LogUtil.Dispose();
        }
    }
}
