using System;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public delegate void OnInitFinished(bool result);
    public delegate void OnUnloadFinished();

    public delegate void OnLoadAssetProgress(string address, float progress, SystemObject userdata);
    public delegate void OnLoadAssetComplete(string address, UnityObject uObject, SystemObject userdata);

    public delegate void OnLoadAssetsProgress(string[] addresses, float[] progress, SystemObject userdata);
    public delegate void OnLoadAssetsComplete(string[] addresses, UnityObject[] uObjects, SystemObject userdata);

    public enum LoaderState
    {
        None = 0,
        Initializing,
        Initialized,
        Running,
        Error,
    }

    public enum AsyncPriority
    {
        VeryLow = 100,
        Low = 200,
        Default = 300,
        High = 400,
        VeryHigh = 500,
    }

    public enum AsyncState
    {
        None = 0,
        WaitingForStart,
        Loading,
        LoadFinished,
        WaitingForInstance,
        Instancing,
        InstanceFinished,
    }

    public interface ILoader
    {
        LoaderState State { get; }
        int OperationMaxCount { get; set; }

        void DoInitialize(AssetDetailConfig detailConfig, OnInitFinished initCallback, params SystemObject[] values);

        UnityObject[] LoadAssetsSync(string[] addresses);

        UnityObject[] InstanceAssetsSync(string[] addresses);

        AsyncResult LoadAssetsAsync(
            string[] addresses,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            OnLoadAssetsProgress progressesCallback,
            OnLoadAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata);

        AsyncResult InstanceAssetsAsync(
            string[] addresses,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            OnLoadAssetsProgress progressesCallback,
            OnLoadAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata);

        void CancelAssetsAsync(AsyncResult result);

        UnityObject InstanceUObject(string address, UnityObject uObject);
        void DestroyUObject(string address,UnityObject uObject);

        void DoUdpate(float deltaTime, float unscaleDeltaTime);

        void UnloadUnusedAssets();

        void UnloadAssets(OnUnloadFinished finishedCallback);

        void DoDestroy();
    }
}
