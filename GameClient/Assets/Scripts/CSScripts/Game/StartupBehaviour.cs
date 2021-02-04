using DotEngine;
using DotEngine.Asset;
using DotEngine.GOP;
using DotEngine.Log;
using DotEngine.Log.Appender;
using DotEngine.Lua;
using DotEngine.Timer;
using DotEngine.Utilities;
using UnityEngine;

namespace Game
{
    public class StartupBehaviour : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 30;

            LogUtil.AddAppender(new UnityConsoleAppender());

            Facade facade = GameFacade.GetInstance();
            AssetService assetService = facade.GetServicer<AssetService>(AssetService.NAME);
#if UNITY_EDITOR
            assetService.InitDatabaseLoader((result) =>
            {
                if(result)
                {
                    OnAssetInitialize();
                }
            });

#else
            string bundleRootDir = "./bundles";
            assetService.InitBundleLoader((result) =>
            {
                LuaEnvService luaEnvService = facade.GetServicer<LuaEnvService>(LuaEnvService.NAME);
                luaEnvService.CallAction(LuaConst.START_FUNCTION_NAME);
            }, bundleRootDir);
#endif

            //TimerService timerService = facade.GetService<TimerService>(TimerService.NAME);
            //handler = timerService.AddIntervalTimer(1, (userdata) =>
            //{
            //    count++;
            //    if(count == 10)
            //    {
            //        timerService.RemoveTimer(handler);
            //    }
            //    LogUtil.LogInfo("Timer Test", "Test Interval"+"   -> "+userdata+"    --  "+count);
            //},"AIT");

            DontDestroyUtility.AddTransform(transform);
        }

        private void OnAssetInitialize()
        {
            Facade facade = GameFacade.GetInstance();
            LuaEnvService luaEnvService = facade.GetServicer<LuaEnvService>(LuaEnvService.NAME);
            luaEnvService.CallAction(LuaConst.START_FUNCTION_NAME);

            //GameObjectPoolService poolService = facade.GetService<GameObjectPoolService>(GameObjectPoolService.NAME);
            //var group = poolService.CreateGroup("TestGroup");

            //AssetService assetService = facade.GetService<AssetService>(AssetService.NAME);
            //assetService.LoadAssetAsync("Cube", (address, uObj, userdata) =>
            //{
            //    var pool = group.CreatePool(address, PoolTemplateType.Prefab, (GameObject)uObj);
            //    pool.SetPreload(100, 2);
            //    pool.SetCull(5, 10);
            //    pool.SetLimit(5, 10);
            //});
        }

        private void Update()
        {
        }


        private int count = 0;
        private TimerInstance handler = null;

        private void OnDestroy()
        {
            LogUtil.Reset();

            Facade facade = GameFacade.GetInstance();
            facade.Dispose();
        }
    }
}
