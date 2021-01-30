using UnityEngine;
using UnityEngine.SceneManagement;

namespace DotEngine.Asset
{
    /// <summary>
    /// 
    /// </summary>
    public class SceneHandler
    {
        /// <summary>
        /// 场景地址
        /// </summary>
        public string Address { get;}
        /// <summary>
        /// 场景名称
        /// </summary>
        public string SceneName { get;}
        /// <summary>
        /// 场景资源的路径
        /// </summary>
        public string ScenePath { get;}

        /// <summary>
        /// 场景加载进度
        /// </summary>
        public float Progress { get; internal set; }
        /// <summary>
        /// 如果是加载场景的话，则存储加载成功后的场景
        /// </summary>
        public Scene TargetScene { get; internal set; }
        /// <summary>
        /// 场景加载或卸载是否完成
        /// </summary>
        public bool IsDone { get; internal set; } = false;

        internal SceneHandler(string address,string sceneName,string scenePath)
        {
            Address = address;
            SceneName = sceneName;
            ScenePath = scenePath;
            Progress = 0.0f;
        }

        /// <summary>
        /// 激活场景的顶层结点
        /// </summary>
        public void ActiveScene()
        {
            if (IsDone && TargetScene.IsValid() && TargetScene.isLoaded)
            {
                GameObject[] gObjs = TargetScene.GetRootGameObjects();
                foreach(var gObj in gObjs)
                {
                    gObj.SetActive(true);
                }
            }
        }
    }
}
