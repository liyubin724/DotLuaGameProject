using DotEngine.Context;
using UnityEngine;

namespace DotEditor.Asset.Post
{
    public class AssetPostRuler : ScriptableObject
    {
        [ContextField(AssetPostContextKeys.ASSET_FILTER_RESULT_KEY, ContextUsage.In, false)]
        protected string[] assetPaths;

        public virtual void Execute() 
        { }
    }
}
