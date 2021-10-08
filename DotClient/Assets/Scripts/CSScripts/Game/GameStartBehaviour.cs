using DotEngine.Core;
using DotEngine.UPool;
using DotEngine.Log;
using DotEngine.Lua;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameStartBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            PersistentUObjectHelper.AddGameObject(gameObject);
            GameLauncher.Startup();
        }

        private void OnDisable()
        {
            GameLauncher.Shutdown();
        }


        //        void Awake()
        //        {
        //            PersistentUObjectHelper.AddGameObject(gameObject);

        //            LogUtil.AddAppender(new UnityConsoleAppender());
        //            LuaLogger.SetHandler(LogUtil.Info, LogUtil.Warning, LogUtil.Error);
        //            //GOPLogger.SetHandler(LogUtil.Info, LogUtil.Warning, LogUtil.Error);


        //            --LuaBridger.GetInstance().Startup();

        //#if UNITY_EDITOR
        //            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        //#endif
        //        }

        //        private void OnDisable()
        //        {
        //#if UNITY_EDITOR
        //            UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        //#endif
        //        }

        //#if UNITY_EDITOR
        //        public void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
        //        {

        //        }
        //#endif
    }

}

