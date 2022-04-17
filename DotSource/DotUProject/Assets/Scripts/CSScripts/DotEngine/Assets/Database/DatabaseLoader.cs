#if UNITY_EDITOR
using DotEngine.Pool;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public class DatabaseLoader : ALoader
    {
        private ElementPool<DatabaseAssetAsyncOperation> assetOperationPool = new ElementPool<DatabaseAssetAsyncOperation>();
        protected override bool OnInitializeUpdate(float deltaTime)
        {
            State = LoaderState.Initialized;
            return true;
        }


        #region Sync
        protected override UnityObject RequestAssetSync(string assetPath)
        {
            return AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityObject));
        }

        #endregion
        protected override void OnAsyncRequestStart(AsyncRequest request)
        {
        }

        protected override void OnAsyncRequestUpdate(AsyncRequest request)
        {
            var result = request.result;
            string[] assetPaths = request.paths;
            for (int i = 0; i < assetPaths.Length; i++)
            {
                if (result.IsDoneAt(i))
                {
                    continue;
                }

                string assetPath = assetPaths[i];
                AssetNode assetNode = assetNodeDic[assetPath];
                if (assetNode.IsLoaded())
                {
                    if (request.isInstance)
                    {
                        request.SetUObject(i, assetNode.CreateInstance());
                    }
                    else
                    {
                        request.SetUObject(i, assetNode.GetAsset());
                    }
                    assetNode.ReleaseRef();
                    continue;
                }
                else
                {
                    if(!operationLDic.TryGetValue(assetPath,out var assetOperation))
                    {
                        assetOperation = assetOperationPool.Get();
                        assetOperation.DoInitilize(assetPath);
                        assetOperation.OnOperationComplete = OnAssetLoadCompleted;

                        operationLDic.Add(assetPath, assetOperation);
                    }
                    
                    request.SetProgress(i, assetOperation.Progress);
                }
            }
            if (result.IsDone())
            {
                if (request.isInstance)
                {
                    request.state = RequestState.InstanceFinished;
                }
                else
                {
                    request.state = RequestState.LoadFinished;
                }
            }
        }

        private void OnAssetLoadCompleted(AAsyncOperation operation)
        {
            DatabaseAssetAsyncOperation assetOperation = (DatabaseAssetAsyncOperation)operation;
            string assetPath = operation.Path;
            if (assetNodeDic.TryGetValue(assetPath, out var assetNode))
            {
                assetNode.SetAsset(assetOperation.GetAsset());
            }
            operationLDic.Remove(assetPath);
            assetOperationPool.Release(assetOperation);
        }

        protected override void OnAsyncRequestEnd(AsyncRequest request)
        {
        }

        protected override void OnAsyncRequestCancel(AsyncRequest request)
        {
        }


        protected override void OnCreateAssetNodeAsync(AssetNode assetNode)
        {
        }

        protected override void OnCreateAssetNodeSync(AssetNode assetNode)
        {
        }

        protected override void OnReleaseAssetNode(AssetNode assetNode)
        {
        }

        protected override bool OnUnloadAssetsUpdate()
        {
            return true;
        }

        
    }
}
#endif