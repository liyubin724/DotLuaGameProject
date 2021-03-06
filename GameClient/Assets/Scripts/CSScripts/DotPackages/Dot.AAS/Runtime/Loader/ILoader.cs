using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemObject = System.Object;
using UnityObject = UnityEngine.Object;

namespace DotEngine.AAS.Loader
{
    public interface ILoader
    {
        int MaxAssetLoadingCount { get; set; }
        int MaxAssetIntancingCount { get; set; }

        void InitLoader();

        LoaderHandle LoadAssetAsync(
            string address,
            LoadProgress progress,
            LoadComplete complete,
            LoaderPriority priority,
            SystemObject userdata);

        LoaderHandle InstanceAssetAsync(
            string address,
            LoadProgress progress,
            LoadComplete complete,
            LoaderPriority priority,
            SystemObject userdata);

        LoaderHandle BatchLoadAssetAsync(
            string[] addresses,
            LoadProgress progress,
            LoadComplete complete,
            BatchLoadProgress batchProgress,
            BatchLoadComplete batchComplete,
            LoaderPriority priority,
            SystemObject userdata);

        LoaderHandle BatchInstanceAssetAsync(
            string[] addresses,
            LoadProgress progress,
            LoadComplete complete,
            BatchLoadProgress batchProgress,
            BatchLoadComplete batchComplete,
            LoaderPriority priority,
            SystemObject userdata);

        LoaderHandle LoadAssetByLabelAsync(
            string label,
            LoadProgress progress,
            LoadComplete complete,
            BatchLoadProgress batchProgress,
            BatchLoadComplete batchComplete,
            LoaderPriority priority,
            SystemObject userdata);

        LoaderHandle InstanceAssetByLabelAsync(
            string label,
            LoadProgress progress,
            LoadComplete complete,
            BatchLoadProgress batchProgress,
            BatchLoadComplete batchComplete,
            LoaderPriority priority,
            SystemObject userdata);

        void UnloadLoader(LoaderHandle handle, bool destroyIfInstance = false);

        void DoUpdate(float deltaTime, float unscaleDeltaTime);
    }
}
