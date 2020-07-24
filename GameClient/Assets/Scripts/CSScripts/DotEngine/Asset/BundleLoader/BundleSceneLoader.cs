using UnityEngine.SceneManagement;

namespace DotEngine.Asset
{
    public class BundleSceneLoader : ASceneLoader
    {
        private static readonly float ASSET_LOADING_RATE = 0.95f;

        private AssetHandler assetHandler = null;
        public BundleSceneLoader(AAssetLoader loader) : base(loader)
        {
        }

        protected override void UpdateLoaderData()
        {
            if (currentLoaderData.state == SceneLoaderDataState.Load)
            {
                OnStartLoadScene();
            }
            else if (currentLoaderData.state == SceneLoaderDataState.Unload)
            {
                OnStartUnloadScene();
            }else if(currentLoaderData.state == SceneLoaderDataState.Loading)
            {
                OnLoadingScene();
            }else if(currentLoaderData.state == SceneLoaderDataState.Instancing)
            {
                OnInstancingScene();   
            }else if(currentLoaderData.state == SceneLoaderDataState.Unloading)
            {
                OnUnloadingScene();
            }
        }

        private void OnStartLoadScene()
        {
            currentLoaderData.state = SceneLoaderDataState.Loading;
            assetHandler = assetLoader.LoadBatchAssetAsync(string.Empty,
                new string[] { currentLoaderData.address }, false,
                null, null, null, null, AssetLoaderPriority.Default, null);
        }

        private void OnLoadingScene()
        {
            if(assetHandler.IsDone)
            {
                currentLoaderData.DoProgress(ASSET_LOADING_RATE);

                assetHandler = null;
                currentLoaderData.state = SceneLoaderDataState.Instancing;
                asyncOperation = SceneManager.LoadSceneAsync(currentLoaderData.sceneName, currentLoaderData.sceneMode);
            }else
            {
                currentLoaderData.DoProgress(assetHandler.Progress * ASSET_LOADING_RATE);
            }
        }

        private void OnInstancingScene()
        {
            if (asyncOperation.isDone)
            {
                assetLoader.UnloadAssetByAddress(currentLoaderData.address);

                Scene scene = SceneManager.GetSceneByName(currentLoaderData.sceneName);
                currentLoaderData.DoComplete(scene);
                currentLoaderData = null;
                asyncOperation = null;
            }
            else
            {
                currentLoaderData.DoProgress(asyncOperation.progress * (1 - ASSET_LOADING_RATE) + ASSET_LOADING_RATE);
            }
        }

        private void OnStartUnloadScene()
        {
            currentLoaderData.state = SceneLoaderDataState.Unloading;
            asyncOperation = SceneManager.UnloadSceneAsync(currentLoaderData.sceneName);
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
