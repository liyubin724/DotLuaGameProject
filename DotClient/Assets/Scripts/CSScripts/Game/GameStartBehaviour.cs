using DotEngine.Core;
using DotEngine.GOP;
using DotEngine.Log;
using DotEngine.Lua;
using DotEngine.NetCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameStartBehaviour : MonoBehaviour
    {
        public static void Startup()
        {
            LuaEnvManager.GetInstance().Startup();
        }

        public static void Shuntdown()
        {
            LuaEnvManager.GetInstance().Dispose();
        }

        void Awake()
        {
            PersistentUObjectHelper.AddGameObject(gameObject);

            LogUtil.AddAppender(new UnityConsoleAppender());
            LuaLogger.SetHandler(LogUtil.Info, LogUtil.Warning, LogUtil.Error);
            NetLogger.SetHandler(LogUtil.Info, LogUtil.Warning, LogUtil.Error);
            GOPLogger.SetHandler(LogUtil.Info, LogUtil.Warning, LogUtil.Error);

            
            LuaEnvManager.GetInstance().Startup();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
        }

        private void OnDisable()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
#endif
        }

#if UNITY_EDITOR
        public void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
        {

        }
#endif
    }

}

