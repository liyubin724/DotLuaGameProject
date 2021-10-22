using System;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public delegate void OnInitFinished(bool result);
    public delegate void OnUnloadUnusedFinished();

    public delegate void OnLoadAssetProgress(string address, float progress, SystemObject userdata);
    public delegate void OnLoadAssetComplete(string address, UnityObject uObject, SystemObject userdata);

    public delegate void OnLoadAssetsProgress(string[] addresses, float[] progress, SystemObject userdata);
    public delegate void OnLoadAssetsComplete(string[] addresses, float[] progresses, SystemObject userdata);

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
        WaitingForInstance,
        instancing,
        Finished,
    }

    public interface ILoader
    {
        LoaderState State { get; }
        int AsyncOperationMaxCount { get; set; }
        int InstanceMaxCount { get; set; }

        void DoInitialize(AssetDetailConfig detailConfig, OnInitFinished initCallback, params SystemObject[] values);

        UnityObject LoadAssetByAddress(string address);
        UnityObject InstanceAssetByAddress(string address);
        UnityObject[] LoadAssetsByAddress(string[] addresses);
        UnityObject[] InstanceAssetsByAdress(string[] addresses);

        UnityObject[] LoadAssetsByLabel(string label);
        UnityObject[] InstanceAssetsByLabel(string label);

        AsyncResult LoadAssetAsyncByAddress(
            string address,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            AsyncPriority priority,
            SystemObject userdata);
        AsyncResult InstanceAssetAsyncByAddress(
            string address,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            AsyncPriority priority, 
            SystemObject userdata);

        AsyncResult LoadAssetsAsyncByAddress(
            string[] addresses,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            OnLoadAssetsProgress progressesCallback,
            OnLoadAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata);
        AsyncResult InstanceAssetsAsyncByAdress(
            string[] addresses,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            OnLoadAssetsProgress progressesCallback,
            OnLoadAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata);
        AsyncResult LoadAssetsAsyncByLabel(
            string label,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            OnLoadAssetsProgress progressesCallback,
            OnLoadAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata);
        AsyncResult InstanceAssetsAsyncByLabel(
            string label,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            OnLoadAssetsProgress progressesCallback,
            OnLoadAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata);

        UnityObject InstanceUObject(string address, UnityObject uObject);
        void DestroyUObject(string address,UnityObject uObject);

        void DoUdpate(float deltaTime, float unscaleDeltaTime);

        void UnloadUnusedAssets(OnUnloadUnusedFinished unloadCallback);

        void DoDestroy();
    }
}
