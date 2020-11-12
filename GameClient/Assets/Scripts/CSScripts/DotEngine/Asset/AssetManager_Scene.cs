using DotEngine.Log;
using UnityEngine.SceneManagement;
using SystemObject = System.Object;

namespace DotEngine.Asset
{
    /// <summary>
    /// 场景加载完成后回调
    /// </summary>
    /// <param name="address">场景地址</param>
    /// <param name="scene">加载到的场景对象</param>
    /// <param name="userData">自定义参数</param>
    public delegate void OnSceneLoadComplete(string address, Scene scene, SystemObject userData);
    /// <summary>
    /// 场景加载进度回调
    /// </summary>
    /// <param name="address">场景地址</param>
    /// <param name="progress">加载进度</param>
    /// <param name="userData">自定义参数</param>
    public delegate void OnSceneLoadProgress(string address, float progress, SystemObject userData);

    public partial class AssetManager
    {
        /// <summary>
        /// 异步加载并初始化场景。场景的加载分为两步，一是加载场景资源及其依赖资源，二是初始化场景
        /// </summary>
        /// <param name="address">场景地址</param>
        /// <param name="complete">加载及初始化完成后回调</param>
        /// <param name="progress">加载进度回调</param>
        /// <param name="mode">加载模式<see cref="LoadSceneMode"/></param>
        /// <param name="activateOnLoad">场景加载完毕后是否立即激活所有的根结点</param>
        /// <param name="userData">自定义参数</param>
        /// <returns></returns>
        public SceneHandler LoadSceneAsync(string address,
            OnSceneLoadProgress progress,
            OnSceneLoadComplete complete,
            LoadSceneMode mode = LoadSceneMode.Single,
            bool activateOnLoad = true,
            SystemObject userData = null)
        {
            if(sceneLoader == null)
            {
                LogUtil.Error(AssetConst.LOGGER_NAME, "AssetManager::LoadSceneAsync->loader hasn't been inited");
                return null;
            }

             return sceneLoader.LoadSceneAsync(address, progress, complete,  mode, activateOnLoad, userData);
        }

        /// <summary>
        /// 卸载指定的场景
        /// </summary>
        /// <param name="address">场景地址</param>
        /// <param name="complete">卸载完毕后回调</param>
        /// <param name="progress">卸载进度回调</param>
        /// <param name="userData">自定义参数</param>
        /// <returns></returns>
        public SceneHandler UnloadSceneAsync(string address,
            OnSceneLoadProgress progress,
            OnSceneLoadComplete complete,
            SystemObject userData = null)
        {
            if (sceneLoader == null)
            {
                LogUtil.Error(AssetConst.LOGGER_NAME, "AssetManager::UnloadSceneAsync->loader hasn't been inited");
                return null;
            }
            return sceneLoader.UnloadSceneAsync(address, progress, complete, userData);
        }
    }
}
