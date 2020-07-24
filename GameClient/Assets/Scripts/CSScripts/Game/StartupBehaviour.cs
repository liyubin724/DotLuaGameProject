using DotEngine;
using DotEngine.Log;
using DotEngine.Lua;
using DotEngine.Utilities;
using UnityEngine;

namespace Game
{
    public class StartupBehaviour : MonoBehaviour
    {
        public string luaEnvName = "game";
        public string[] luaPreloadScripts = new string[]
        {
            "DotLua/Startup"
        };
        public string luaMgrScript = "Game/GameEnvManager";

        private void Awake()
        {
            DotEngine.Log.ILogger logger = new UnityLogger();
            LogUtil.SetLogger(logger);

            Facade facade = GameFacade.GetInstance();
            LuaEnvService luaEnvService = facade.GetService<LuaEnvService>(LuaEnvService.NAME);

            luaEnvService.CreateEnv(luaEnvName, new string[] { LuaConst.GetScriptPathFormat() }, luaPreloadScripts, luaMgrScript);

            DontDestroyHandler.AddTransform(transform);
        }

        private void OnDestroy()
        {
            LogUtil.DisposeLogger();
        }
    }
}
