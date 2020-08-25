using System.Collections.Generic;
using UnityEngine;

namespace DotEditor.Asset.Post
{
    public class AssetPostProcess : ScriptableObject
    {
        public AssetPostFilter Filter;
        public List<AssetPostRuler> rulers = new List<AssetPostRuler>();

        public void Process()
        {
            string[] assets = Filter.GetResults();
            if(assets!=null && assets.Length>0)
            {
                foreach(var asset in assets)
                {
                    Process(asset);
                }
            }
        }

        public void Process(string assetPath)
        {
            foreach(var ruler in rulers)
            {
                ruler.Execute(assetPath);
            }
        }
    }
}
