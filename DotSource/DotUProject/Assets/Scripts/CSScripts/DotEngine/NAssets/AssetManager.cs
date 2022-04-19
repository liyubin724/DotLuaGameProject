using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.NAssets
{
    public delegate void AssetInitFinished(bool result);
    public delegate void AssetUnloadFinished();

    public delegate void AssetAsyncProgress(int id, string address, float progress, SystemObject userdata);
    public delegate void AssetAsyncComplete(int id, string address, UnityObject uObject, SystemObject userdata);

    public delegate void AssetBatchAsyncProgress(int id, string[] addresses, float[] progresses, SystemObject userdata);
    public delegate void AssetBatchAsyncComplete(int id, string[] addresses, UnityObject[] uObjects, SystemObject userdata);

    public enum AssetAsyncPriority
    {
        VeryLow = 100,
        Low = 200,
        Default = 300,
        High = 400,
        VeryHigh = 500,
    }

    public class AssetManager : Singleton<AssetManager>
    {
        protected override void OnInit()
        {
            base.OnInit();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
    }
}
