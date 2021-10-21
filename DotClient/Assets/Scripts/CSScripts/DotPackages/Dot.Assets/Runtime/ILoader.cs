using System;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public delegate void OnLoadAssetProgress(string address, float progress, SystemObject userdata);
    public delegate void OnLoadAssetComplete(string address, UnityObject uObject, SystemObject userdata);

    public delegate void OnLoadAssetsProgress(string[] addresses, float[] progress, SystemObject userdata);
    public delegate void OnLoadAssetsComplete(string[] addresses, float[] progresses, SystemObject userdata);

    public enum AsyncPriority
    {
        VeryLow = 100,
        Low = 200,
        Default = 300,
        High = 400,
        VeryHigh = 500,
    }

    public interface ILoader
    {
        void DoInitialize(AssetDetailConfig detailConfig, Action initCallback, params SystemObject[] objects);

        UnityObject[] LoadAssetsByAddress(string[] addresses);
        UnityObject[] InstanceAssetsByAddress(string[] addresses);

        int LoadAssetsAsync(string[] addresses,
            OnLoadAssetProgress assetLoadProgress,
            OnLoadAssetComplete assetLoadComplete,
            OnLoadAssetsProgress assetsLoadProgress,
            OnLoadAssetsComplete assetsLoadComplete,
            AsyncPriority priority,
            SystemObject userdata);

        int InstanceAssetsAsync(string[] addresses,
            OnLoadAssetProgress assetLoadProgress,
            OnLoadAssetComplete assetLoadComplete,
            OnLoadAssetsProgress assetsLoadProgress,
            OnLoadAssetsComplete assetsLoadComplete,
            AsyncPriority priority,
            SystemObject userdata);

        void DoUdpate(float deltaTime, float unscaleDeltaTime);

        void UnloadUnusedAssets(Action unloadCallback);

        void DoDestroy();
    }
}
