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
        private ILoader assetLoader = null;

        private void Awake()
        {
            PersistentUObjectHelper.AddGameObject(gameObject);
            GameLauncher.Startup();

#if UNITY_EDITOR
            string assetFilePath = AssetConst.GetAssetDetailConfigPathInProject();
            string content = AssetDatabase.LoadAssetAtPath<TextAsset>(assetFilePath).text;
            AssetDetailConfig assetDetailConfig = JSONReader.ReadFromText<AssetDetailConfig>(content);
            assetDetailConfig.InitConfig();

            assetLoader = new DatabaseLoader();
            assetLoader.DoInitialize(assetDetailConfig, (result) =>
            {
                string assetPath = "TestPrefab";
                UnityObject[] uObjects = assetLoader.InstanceAssetsSync(new string[] { assetPath });
                if(uObjects == null || uObjects.Length == 0)
                {
                    Debug.LogError("FFFFFFFFFFFFFFFFFFFFFF");
                }else
                {
                    UnityObject uObject = uObjects[0];
                    (uObject as GameObject).name = "Test Prefab";
                    (uObject as GameObject).transform.position = new Vector3(0, 0, 0);

                }

                assetLoader.LoadAssetsAsync(new string[] { "login_panel" }, null, (address, uObject, userdata) =>
                {
                    if(uObject!=null)
                    {
                        UnityObject instance = assetLoader.InstanceUObject(address, uObject);
                        (instance as GameObject).name = "Test Login Panel";
                    }else
                    {
                        Debug.LogError("DDDDDDDDDDDDD");
                    }

                }, null, null, AsyncPriority.Default, null);
            });
#endif
        }

        private void Update()
        {
            if(assetLoader!=null)
            {
                assetLoader.DoUdpate(Time.deltaTime, Time.unscaledDeltaTime);
            }
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

