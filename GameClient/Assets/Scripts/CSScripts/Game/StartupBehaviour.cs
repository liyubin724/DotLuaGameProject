using DotEngine;
using DotEngine.Asset;
using DotEngine.Log;
using DotEngine.Lua;
using DotEngine.Utilities;
using UnityEngine;

namespace Game
{
    public class StartupBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            DotEngine.Log.ILogger logger = new UnityLogger();
            LogUtil.SetLogger(logger);

            Facade facade = GameFacade.GetInstance();
            AssetService assetService = facade.GetService<AssetService>(AssetService.NAME);
            assetService.InitDatabaseLoader((result) =>
            {
                LuaEnvService luaEnvService = facade.GetService<LuaEnvService>(LuaEnvService.NAME);
                luaEnvService.CallAction(LuaConst.STARTUP_FUNCTION_NAME);
            });

            DontDestroyHandler.AddTransform(transform);
        }

        private void OnDestroy()
        {
            LogUtil.DisposeLogger();
        }
    }
}
