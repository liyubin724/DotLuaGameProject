using DotEngine.Generic;
using DotEngine.Pool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public abstract class ALoader : ILoader
    {
        public LoaderState State { get; protected set; } = LoaderState.None;

        protected AssetDetailConfig assetDetailConfig = null;
        protected Action initializedCallback = null;

        protected Dictionary<string, AssetNode> assetNodeDic = new Dictionary<string, AssetNode>();

        public void DoInitialize(AssetDetailConfig detailConfig, Action initCallback, params SystemObject[] values)
        {
            State = LoaderState.Initializing;

            assetDetailConfig = detailConfig;
            initializedCallback = initCallback;

            OnInitialize(values);
        }

        public UnityObject LoadAssetByAddress(string address)
        {
            string assetPath = assetDetailConfig.GetPathByAddress(address);
            AssetNode assetNode = GetAssetNode(assetPath);

            return assetNode.GetAsset();
        }

        public UnityObject InstanceAssetByAddress(string address)
        {
            string assetPath = assetDetailConfig.GetPathByAddress(address);
            AssetNode assetNode = GetAssetNode(assetPath);

            return assetNode.GetInstance();
        }

        public UnityObject[] LoadAssetsByAddress(string[] addresses)
        {
            UnityObject[] results = new UnityObject[addresses.Length];
            for (int i = 0; i < addresses.Length; i++)
            {
                results[i] = LoadAssetByAddress(addresses[i]);
            }
            return results;
        }

        public UnityObject[] InstanceAssetsByAdress(string[] addresses)
        {
            UnityObject[] results = new UnityObject[addresses.Length];
            for (int i = 0; i < addresses.Length; i++)
            {
                results[i] = InstanceAssetByAddress(addresses[i]);
            }
            return results;
        }

        public UnityObject[] LoadAssetsByLabel(string label)
        {
            string[] addresses = assetDetailConfig.GetAddressesByLabel(label);
            return LoadAssetsByAddress(addresses);
        }

        public UnityObject[] InstanceAssetsByLabel(string label)
        {
            string[] addresses = assetDetailConfig.GetAddressesByLabel(label);
            return InstanceAssetsByAdress(addresses);
        }

        public AssetAsyncResult LoadAssetAsyncByAddress(
            string address,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            return null;
        }

        public AssetAsyncResult InstanceAssetAsyncByAddress(
            string address,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            return null;
        }

        public AssetAsyncResult LoadAssetsAsyncByAddress(
            string[] addresses,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            OnLoadAssetsProgress progressesCallback,
            OnLoadAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            return null;
        }

        public AssetAsyncResult InstanceAssetsAsyncByAdress(
            string[] addresses,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            OnLoadAssetsProgress progressesCallback,
            OnLoadAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            return null;
        }

        public AssetAsyncResult LoadAssetsAsyncByLabel(
            string label,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            OnLoadAssetsProgress progressesCallback,
            OnLoadAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            return null;
        }

        public AssetAsyncResult InstanceAssetsAsyncByLabel(
            string label,
            OnLoadAssetProgress progressCallback,
            OnLoadAssetComplete completeCallback,
            OnLoadAssetsProgress progressesCallback,
            OnLoadAssetsComplete completesCallback,
            AsyncPriority priority,
            SystemObject userdata)
        {
            return null;
        }

        public UnityObject InstanceUObject(string address, UnityObject uObject)
        {
            return null;
        }

        public void UnloadUnusedAssets(Action unloadCallback)
        {

        }


        public void DoUdpate(float deltaTime,float unscaleDeltaTime)
        {
            if(State == LoaderState.Initializing)
            {
                OnInitializeUpdate(deltaTime);
                return;
            }
            if(State == LoaderState.Initialized)
            {
                State = LoaderState.Running;
                initializedCallback?.Invoke();
                return;
            }
            if(State == LoaderState.Running)
            {

            }
        }

        public void DoDestroy()
        {

        }

        private AssetNode GetAssetNode(string assetPath)
        {
            if (assetNodeDic.TryGetValue(assetPath, out var assetNode))
            {
                if (assetNode != null && assetNode.IsAssetValid())
                {
                    return assetNode;
                }
            }

            UnityObject uObject = LoadAsset(assetPath);
            if (assetNode == null)
            {
                assetNode = new AssetNode
                {
                    Path = assetPath
                };

                assetNodeDic.Add(assetPath, assetNode);
            }
            assetNode.SetAsset(uObject);

            return assetNode;
        }

        protected abstract void OnInitialize(params SystemObject[] values);
        protected abstract void OnInitializeUpdate(float deltaTime);

        protected abstract UnityObject LoadAsset(string assetPath);
    }
}
