using DotEngine.Log;
using DotEngine.Lua;
using DotEngine.Utilities;
using UnityEngine;
using XLua;

namespace Game
{
    public class StartupBehaviour : MonoBehaviour
    {
        private void Start()
        {
            Application.targetFrameRate = 30;
            DontDestroyUtility.AddTransform(transform);

            LogUtil.AddAppender(new UnityConsoleAppender());
            new LuaEnvManager();

        }

        private void Update()
        {
        }

        private void OnDestroy()
        {
            LogUtil.Reset();
        }
    }
}
