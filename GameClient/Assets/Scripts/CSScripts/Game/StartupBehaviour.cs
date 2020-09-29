using DotEngine;
using DotEngine.Asset;
using DotEngine.GOPool;
using DotEngine.Log;
using DotEngine.Lua;
using DotEngine.Monitor;
using DotEngine.Monitor.Recorder;
using DotEngine.Monitor.Sampler;
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

            DotEngine.Log.ILogger logger = new CombineLogger();
            LogUtil.SetLogger(logger);

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
                LuaEnvService luaEnvService = facade.GetService<LuaEnvService>(LuaEnvService.NAME);
                luaEnvService.CallAction(LuaConst.STARTUP_FUNCTION_NAME);
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

            DontDestroyHandler.AddTransform(transform);

            MonitorSystem.GetInstance().OpenSamplers(new MonitorSamplerType[] { 
                MonitorSamplerType.FPS,
                MonitorSamplerType.Log,
                MonitorSamplerType.ProfilerMemory,
                MonitorSamplerType.USystemDevice,
                MonitorSamplerType.XLuaMemory,
                MonitorSamplerType.FPS,
            });

            MonitorSystem.GetInstance().OpenRecorder(MonitorRecorderType.File, "D:/logs");
            //MonitorSystem.GetInstance().OpenRecorder(MonitorRecorderType.Console);
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
            MonitorSystem.GetInstance().DoUpdate(Time.deltaTime);
        }


        private int count = 0;
        private TimerHandler handler = null;

        private void OnDestroy()
        {
            MonitorSystem.GetInstance().Dispose();
            LogUtil.DisposeLogger();
            Facade facade = GameFacade.GetInstance();
            facade.Dispose();
        }
    }
}
