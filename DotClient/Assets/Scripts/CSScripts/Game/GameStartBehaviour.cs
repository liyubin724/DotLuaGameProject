using DotEngine.Core;
using DotEngine.UPool;
using DotEngine.Log;
using DotEngine.Lua;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DotEngine.Assets;
using DotEngine.Core.IO;
using UnityObject = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game
{
    public class GameStartBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            PersistentUObjectHelper.AddGameObject(gameObject);
            GameLauncher.Startup();

            LoaderUtill.Init((result) =>
            {
                string assetPath = "TestPrefab";
                UnityObject[] uObjects = LoaderUtill.InstanceAssetsSync(new string[] { assetPath });
                if (uObjects == null || uObjects.Length == 0)
                {
                    Debug.LogError("FFFFFFFFFFFFFFFFFFFFFF");
                }
                else
                {
                    UnityObject uObject = uObjects[0];
                    (uObject as GameObject).name = "Test Prefab";
                    (uObject as GameObject).transform.position = new Vector3(0, 0, 0);

                }

                LoaderUtill.LoadAssetsAsync(new string[] { "login_panel" }, null, (index, address, uObject, userdata) =>
                {
                    if (uObject != null)
                    {
                        UnityObject instance = LoaderUtill.InstanceUObject(address, uObject);
                        (instance as GameObject).name = "Test Login Panel";
                    }
                    else
                    {
                        Debug.LogError("DDDDDDDDDDDDD");
                    }

                }, null, null, AsyncPriority.Default, null);
            });
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

