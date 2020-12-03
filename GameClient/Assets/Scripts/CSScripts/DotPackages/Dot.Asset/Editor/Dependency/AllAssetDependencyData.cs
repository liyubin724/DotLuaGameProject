using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DotEditor.Asset.Dependency
{
    public class AllAssetDependencyData : ScriptableObject, ISerializationCallbackReceiver
    {
        public List<AssetDependencyData> assetDatas = new List<AssetDependencyData>();

        private Dictionary<string, AssetDependencyData> assetDataDic = new Dictionary<string, AssetDependencyData>();
        public void OnAfterDeserialize()
        {
            assetDataDic.Clear();
            foreach(var assetData in assetDatas)
            {
                assetDataDic.Add(assetData.assetPath, assetData);
            }
        }

        public void OnBeforeSerialize()
        {
            assetDatas.Clear();
            assetDatas = assetDataDic.Values.ToList();
        }

        public void Clear()
        {
            assetDataDic.Clear();
            assetDatas.Clear();
        }

        public bool HasData(string assetPath)
        {
            return assetDataDic.ContainsKey(assetPath);
        }

        public AssetDependencyData GetData(string assetPath)
        {
            if(assetDataDic.TryGetValue(assetPath,out var data))
            {
                return data;
            }
            return null;
        }

        public void AddData(AssetDependencyData data)
        {
            if(assetDataDic.ContainsKey(data.assetPath))
            {
                assetDataDic[data.assetPath] = data;
            }else
            {
                assetDataDic.Add(data.assetPath, data);
            }
        }
    }
}
