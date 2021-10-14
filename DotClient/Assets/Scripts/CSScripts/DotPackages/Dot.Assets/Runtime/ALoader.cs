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
        Running,
        Disposing,
    }

    public abstract class ALoader
    {
        public int MaxAsycOperationCount { get; set; } = 10;
        public int MaxInstanceOperationCount { get; set; } = 5;

        public void DoInitialize(Action initCallback,params SystemObject[] objects)
        {

        }

        public UnityObject[] LoadAssets(string[] assetAddresses)
        {
            UnityObject[] objects = new UnityObject[assetAddresses.Length];
            for (int i = 0; i < assetAddresses.Length; i++)
            {
                objects[i] = LoadAsset(assetAddresses[i]);
            }

            return objects;
        }


        public UnityObject[] InstanceAssets(string[] assetAddresses)
        {
            UnityObject[] objects = LoadAssets(assetAddresses);
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

        protected abstract UnityObject LoadAsset(string assetAddress);
        protected abstract UnityObject InstanceAsset(string assetAddress, UnityObject uObject);
        protected virtual string GetAssetPath(string assetAddress)
        {
            return assetAddress;
        }
    }
}
