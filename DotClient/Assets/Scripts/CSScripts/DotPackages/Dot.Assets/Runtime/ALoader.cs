using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.Assets
{
    public delegate void AsyncLoadAssetStart(string address, SystemObject userdata);
    public delegate void AsyncLoadAssetComplete(string address, UnityObject uObj, SystemObject userdata);
    public delegate void AsyncLoadAssetProgress(string address, float progress, SystemObject userdata);

    public delegate void LoadAssetStart()

    public enum AsyncPriority
    {
        VeryLow = 100,
        Low = 200,
        Default = 300,
        High = 400,
        VeryHigh = 500,
    }

    public enum LoaderState
    {
        None = 0,
        Initializing,
        Initialized,
        Running,
    }

    public abstract class ALoader
    {
        public int MaxAsycOperationCount { get; set; } = 10;
        public int MaxInstanceOperationCount { get; set; } = 5;

        protected AssetDetailConfig assetDetailConfig = null;
        protected Action initCallback = null;
        protected LoaderState State { get; set; } = LoaderState.None;
        public virtual void DoInitialize(AssetDetailConfig detailConfig, Action completedCallback,params SystemObject[] objects)
        {
            assetDetailConfig = detailConfig;
            initCallback = completedCallback;
            State = LoaderState.Initializing;
        }

        protected abstract void DoInitializeUpdate();

        public void DoUpdate(float deltaTime,float unscaleDeltaTime)
        {
            if(State == LoaderState.None)
            {
                return;
            }else if(State == LoaderState.Initializing)
            {
                DoInitializeUpdate();
            }else if(State == LoaderState.Initialized)
            {
                State = LoaderState.Running;
                initCallback?.Invoke();
            }else if(State == LoaderState.Running)
            {

            }
        }

        public UnityObject[] LoadByAddresses(string[] assetAddresses)
        {
            UnityObject[] objects = new UnityObject[assetAddresses.Length];
            for (int i = 0; i < assetAddresses.Length; i++)
            {
                objects[i] = LoadAsset(assetAddresses[i]);
            }

            return objects;
        }

        public UnityObject[] LoadByLabel(string label)
        {
            string[] addresses = assetDetailConfig.GetAddressesByLabel(label);
            return LoadByAddresses(addresses);
        }


        public UnityObject[] InstanceByAddresses(string[] assetAddresses)
        {
            UnityObject[] objects = LoadByAddresses(assetAddresses);
            for (int i = 0; i < assetAddresses.Length; i++)
            {
                UnityObject uObject = objects[i];
                if(uObject !=null)
                {
                    objects[i] = InstanceAsset(assetAddresses[i], uObject);
                }
            }
            return objects;
        }

        public UnityObject[] InstanceByLabel(string label)
        {
            string[] addresses = assetDetailConfig.GetAddressesByLabel(label);
            return InstanceByAddresses(addresses);
        }

        protected abstract UnityObject LoadAsset(string assetAddress);
        protected abstract UnityObject InstanceAsset(string assetAddress, UnityObject uObject);
        protected virtual string GetAssetPath(string assetAddress)
        {
            return assetAddress;
        }
    }
}
