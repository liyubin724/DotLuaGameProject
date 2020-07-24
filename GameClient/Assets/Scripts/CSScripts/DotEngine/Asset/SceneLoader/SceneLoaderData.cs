using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using SystemObject = System.Object;

namespace DotEngine.Asset
{
    /// <summary>
    /// 场景加载或卸载时状态
    /// </summary>
    public enum SceneLoaderDataState
    {
        None = 0,
        
        Load,
        Unload,

        Loading,
        Unloading,
        
        Instancing,
        Finished,
    }

    public class SceneLoaderData
    {
        internal string address;
        internal string scenePath;
        internal string sceneName;
        internal OnSceneLoadComplete completeCallback = null;
        internal OnSceneLoadProgress progressCallback = null;
        internal SystemObject userData =null;
        internal LoadSceneMode sceneMode = LoadSceneMode.Single;
        internal bool isActiveWhenLoaded = true;

        internal SceneHandler handler;
        internal SceneLoaderDataState state = SceneLoaderDataState.None;

        public void InitLoadData(
            string address,
            string path,
            OnSceneLoadComplete complete,
            OnSceneLoadProgress progress,
            LoadSceneMode sceneMode,
            bool isActive,
            SystemObject userData)
        {
            this.address = address;
            this.scenePath = path;
            this.sceneName = Path.GetFileNameWithoutExtension(path);
            completeCallback = complete;
            progressCallback = progress;
            this.sceneMode = sceneMode;
            this.isActiveWhenLoaded = isActive;
            this.userData = userData;

            handler = new SceneHandler(address, sceneName, scenePath);
            state = SceneLoaderDataState.Load;
        }

        public void InitUnloadData(
            string address, 
            string path,
            OnSceneLoadComplete complete,
            OnSceneLoadProgress progress,
            SystemObject userData)
        {
            this.address = address;
            this.scenePath = path;
            this.sceneName = Path.GetFileNameWithoutExtension(path);
            completeCallback = complete;
            progressCallback = progress;
            this.userData = userData;

            handler = new SceneHandler(address,sceneName,scenePath);
            state = SceneLoaderDataState.Unload;
        }

        internal void DoComplete(Scene scene)
        {
            state = SceneLoaderDataState.Finished;

            handler.IsDone = true;
            handler.TargetScene = scene;

            if(!isActiveWhenLoaded && scene.IsValid())
            {
                GameObject[] objs = scene.GetRootGameObjects();
                foreach(var obj in objs)
                {
                    obj.SetActive(false);
                }
            }

            progressCallback?.Invoke(address, 1.0f, userData);
            completeCallback?.Invoke(address, scene, userData);
        }

        internal void DoProgress(float progress)
        {
            handler.Progress = progress;
            progressCallback?.Invoke(address, progress, userData);
        }
    }
}
