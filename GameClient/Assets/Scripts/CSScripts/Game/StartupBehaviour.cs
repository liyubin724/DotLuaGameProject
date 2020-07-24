using DotEngine;
using DotEngine.Log;
using DotEngine.Lua;
using DotEngine.Utilities;
using Game.Lua;
using UnityEngine;

namespace Game
{
    public class StartupBehaviour : MonoBehaviour
    {
        public string[] luaPreloadScripts = new string[0];
        public string luaMgrScript = string.Empty;

        private void Awake()
        {
            DotEngine.Log.ILogger logger = new UnityLogger();
            LogUtil.SetLogger(logger);

            Facade facade = GameFacade.GetInstance();
            GameLuaEnvService envService = new GameLuaEnvService(new string[] { LuaConst.GetScriptPathFormat() }, luaPreloadScripts, luaMgrScript);
            facade.RegisterService(envService);

            DontDestroyHandler.AddTransform(transform);
        }

        private void OnDestroy()
        {
            LogUtil.DisposeLogger();
        }
    }
}
