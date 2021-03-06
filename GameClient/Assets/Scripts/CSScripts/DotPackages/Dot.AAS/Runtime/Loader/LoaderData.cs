using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.AAS.Loader
{
    public delegate void LoadComplete(string address, UnityObject uObj, SystemObject userdata);
    public delegate void LoadProgress(string address, float progress, SystemObject userdata);

    public delegate void BatchLoadComplete(string[] addresses, UnityObject[] uObjs, SystemObject userdata);
    public delegate void BatchLoadProgress(string[] addresses, float[] progresses, SystemObject userdata);

    public enum LoaderState
    {
        None = 0,
        
    }

    public enum LoaderPriority
    {
        VeryLow = 100,
        Low = 200,
        Default = 300,
        High = 400,
        VeryHigh = 500,
    }

    public class LoaderData
    {
        public string Label = null;
        public string[] addresses = new string[0];
        public bool isInstance  = false;

        public LoaderPriority priority = LoaderPriority.Default;

        public LoadComplete completeCallback;
        public LoadProgress progressCallback;

        public BatchLoadComplete batchCompleteCallback;
        public BatchLoadProgress batchProgressCallback;
        public SystemObject userdata;

        public LoaderState state = LoaderState.None;

        public LoaderHandle handle;
    }
}
