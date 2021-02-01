#if UNITY_EDITOR
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace DotEngine.Asset
{
    public class DatabaseSceneLoader : ASceneLoader
    {
        public DatabaseSceneLoader(AAssetLoader loader) : base(loader)
        {
        }

        protected override void UpdateLoaderData()
        {
            if(currentLoaderData.state == SceneLoaderDataState.Load)
            {
                OnStartLoadScene();
            }else if(currentLoaderData.state == SceneLoaderDataState.Unload)
            {
                OnStartUnloadScene();
            }else if(currentLoaderData.state == SceneLoaderDataState.Loading)
            {
                OnLoadingScene();   
            }else if(currentLoaderData.state == SceneLoaderDataState.Unloading)
            {
                OnUnloadingScene();
            }
        }

        private void OnStartLoadScene()
        {
            currentLoaderData.state = SceneLoaderDataState.Loading;
            asyncOperation = EditorSceneManager.LoadSceneAsyncInPlayMode(currentLoaderData.scenePath,
                new LoadSceneParameters(currentLoaderData.sceneMode));
        }

        private void OnStartUnloadScene()
        {
            currentLoaderData.state = SceneLoaderDataState.Unloading;
            asyncOperation = SceneManager.UnloadSceneAsync(currentLoaderData.sceneName);
        }

        private void OnLoadingScene()
        {
            if (asyncOperation.isDone)
            {
                Scene scene = SceneManager.GetSceneByName(currentLoaderData.sceneName);
                currentLoaderData.DoComplete(scene);
                currentLoaderData = null;
                asyncOperation = null;
            }
            else
            {
                currentLoaderData.DoProgress(asyncOperation.progress);
            }
        }

        private void OnUnloadingScene()
        {
            if (asyncOperation.isDone)
            {
                currentLoaderData.DoComplete(new Scene());

                currentLoaderData = null;
                asyncOperation = null;
            }
            else
            {
                currentLoaderData.DoProgress(asyncOperation.progress);
            }
        }
    }
}
#endif