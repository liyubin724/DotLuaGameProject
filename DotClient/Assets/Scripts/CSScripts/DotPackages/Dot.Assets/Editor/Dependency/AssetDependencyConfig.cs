using DotEngine.Core.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace DotEditor.Asset.Dependency
{
    public class AssetDependencyConfig : ISerialization
    {
        public List<AssetDependency> assetDatas = new List<AssetDependency>();

        private Dictionary<string, AssetDependency> assetDataDic = new Dictionary<string, AssetDependency>();
        public void DoDeserialize()
        {
            assetDataDic.Clear();
            foreach(var dependency in assetDatas)
            {
                assetDataDic.Add(dependency.assetPath, dependency);
            }
        }

        public void DoSerialize()
        {
            assetDatas.Clear();
            assetDatas = assetDataDic.Values.ToList();
        }

        public void Clear()
        {
            assetDatas.Clear();
            assetDataDic.Clear();
        }

        public bool HasData(string assetPath)
        {
            return assetDataDic.ContainsKey(assetPath);
        }

        public AssetDependency GetData(string assetPath)
        {
            if(assetDataDic.TryGetValue(assetPath,out var data))
            {
                return data;
            }
            return null;
        }

        public void AddData(AssetDependency data)
        {
            if (assetDataDic.TryGetValue(data.assetPath, out var adData))
            {
                assetDataDic.Remove(data.assetPath);
                assetDatas.Remove(adData);
            }
            assetDataDic[data.assetPath] = data;
            assetDatas.Add(data);
        }

        public void RemoveData(string assetPath)
        {
            if(assetDataDic.TryGetValue(assetPath,out var data))
            {
                assetDataDic.Remove(assetPath);
                assetDatas.Remove(data);
            }
        }

        public AssetDependency[] GetDependAssets(string assetPath)
        {
            if (assetDataDic.TryGetValue(assetPath, out var data))
            {
                return (from depend in data.directlyDepends select assetDataDic[depend]).ToArray();
            }
            return new AssetDependency[0];
        }

        public AssetDependency[] GetBeUsedAssets(string assetPath)
        {
            return (from data in assetDatas
                    where data.allDepends.Contains(assetPath) && data.assetPath != assetPath
                    select data).ToArray();
        }

    }

    [Serializable]
    public class AssetDependency
    {
        public string assetPath;
        public string[] directlyDepends = new string[0];
        public string[] allDepends = new string[0];

        internal UnityObject cachedUObject = null;
        internal Texture2D cachedPreview = null;
    }
}
