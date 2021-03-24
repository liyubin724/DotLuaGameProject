using DotEngine.Log;
using DotEngine.Lua;
using DotEngine.Utilities;
using UnityEngine;
using XLua;

namespace Game
{
    public class StartupBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 30;
            DontDestroyUtility.AddTransform(transform);

            LogUtil.AddAppender(new UnityConsoleAppender());
            LogUtil.AddAppender(new FileLogAppender(@"D:/logs"));
            LuaEnvManager envManager = new LuaEnvManager();
            envManager.Startup();

        }

        private void Update()
        {
        }

        private void OnDestroy()
        {
            LuaEnvManager envManager = LuaEnvManager.GetInstance();
            envManager.Shuntdown();
            
            LogUtil.Reset();
        }
    }
}
