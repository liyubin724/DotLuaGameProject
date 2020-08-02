using DotEngine;
using DotEngine.Asset;
using DotEngine.Log;
using DotEngine.Lua;
using DotEngine.Timer;
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

            TimerService timerService = facade.GetService<TimerService>(TimerService.NAME);
            handler = timerService.AddIntervalTimer(1, (userdata) =>
            {
                count++;
                if(count == 10)
                {
                    timerService.RemoveTimer(handler);
                }
                LogUtil.LogInfo("Timer Test", "Test Interval"+"   -> "+userdata+"    --  "+count);
            },"AIT");

            DontDestroyHandler.AddTransform(transform);
        }

        private int count = 0;
        private TimerHandler handler = null;

        private void OnDestroy()
        {
            LogUtil.DisposeLogger();
        }
    }
}
