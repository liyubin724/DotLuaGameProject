using System;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public delegate void OnInitFinished(bool result);
    public delegate void OnUnloadFinished();

    public delegate void OnAssetProgress(int index, string address, float progress, SystemObject userdata);
    public delegate void OnAssetComplete(int index,string address, UnityObject uObject, SystemObject userdata);

    public delegate void OnAssetsProgress(int index,string[] addresses, float[] progress, SystemObject userdata);
    public delegate void OnAssetsComplete(int index,string[] addresses, UnityObject[] uObjects, SystemObject userdata);

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

    public enum RequestState
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

        UnityObject LoadAssetSync(string address);
        UnityObject InstanceAssetSync(string address);
        UnityObject[] LoadAssetsSync(string[] addresses);
        UnityObject[] InstanceAssetsSync(string[] addresses);
        UnityObject[] LoadAssetsSyncByLabel(string label);
        UnityObject[] InstanceAssetsSyncByLabel(string label);

        int LoadAssetAsync(
            string address,
            OnAssetProgress progressCallback,
            OnAssetComplete completeCallback,
            AsyncPriority priority,
            SystemObject userdata);
        int InstanceAssetAsync(
            string address,
            OnAssetProgress progressCallback,
            OnAssetComplete completeCallback,
            AsyncPriority priority,
            SystemObject userdata);
        int LoadAssetsAsync(
            string[] addresses,
            OnAssetProgress progressCallback,
            OnAssetComplete completeCallback,
            OnAssetsProgress progressesCallback,
            OnAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata);
        int InstanceAssetsAsync(
            string[] addresses,
            OnAssetProgress progressCallback,
            OnAssetComplete completeCallback,
            OnAssetsProgress progressesCallback,
            OnAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata);
        int LoadAssetsAsyncByLabel(
            string label,
            OnAssetProgress progressCallback,
            OnAssetComplete completeCallback,
            OnAssetsProgress progressesCallback,
            OnAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata);
        int InstanceAssetsAsyncByLabel(
            string label,
            OnAssetProgress progressCallback,
            OnAssetComplete completeCallback,
            OnAssetsProgress progressesCallback,
            OnAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata);

        void CancelAssetsAsync(int index);

        UnityObject InstanceUObject(string address, UnityObject uObject);
        void DestroyUObject(string address,UnityObject uObject);

        void DoUdpate(float deltaTime, float unscaleDeltaTime);

        void UnloadUnusedAssets();
        void UnloadAssets(OnUnloadFinished finishedCallback);

        void DoDestroy();
    }
}
