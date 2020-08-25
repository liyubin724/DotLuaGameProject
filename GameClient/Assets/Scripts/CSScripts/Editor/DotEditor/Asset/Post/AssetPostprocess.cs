using DotEngine.Context;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Asset.Post
{
    [CreateAssetMenu(menuName = "Asset Post/Process", fileName = "ap_process",order = 11)]
    public class AssetPostProcess : ScriptableObject
    {
        [Serializable]
        public class AssetPostFilter
        {
            public string Folder = "Assets";
            public bool IsIncludeSub = true;
            public string FileNameRegex = string.Empty;

            public string[] GetResults()
            {
                return DotEditor.Utilities.DirectoryUtility.GetAsset(Folder, IsIncludeSub, FileNameRegex);
            }
        }

        public AssetPostFilter Filter = new AssetPostFilter();
        public List<AssetPostRuler> Rulers = new List<AssetPostRuler>();

        public void Process()
        {
            string[] assets = Filter.GetResults();
            if (assets != null && assets.Length > 0)
            {
                foreach (var asset in assets)
                {
                    Process(asset);
                }
            }
        }

        public void Process(string assetPath)
        {
            StringContext context = new StringContext();
            context.Add("assetPath", assetPath);
            foreach (var ruler in Rulers)
            {
                ruler.Execute(context, assetPath);
            }
            context.Clear();
        }
    }
}
