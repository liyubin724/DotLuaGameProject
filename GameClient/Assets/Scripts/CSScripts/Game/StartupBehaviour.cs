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
            //new LuaEnvManager();

            LuaEnv env = new LuaEnv();
            env.AddLoader(new FileScriptLoader().LoadScript);

            System.Object[] values = env.DoString("require(\"DotLua/OOP/oop\")");
            if(values == null)
            {
                Debug.Log("SSSSSSSSSSSSSSSSS");
            }else
            {
                Debug.Log("SSSFFFFFFFFFFFFFFFFFFF");
            }
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
