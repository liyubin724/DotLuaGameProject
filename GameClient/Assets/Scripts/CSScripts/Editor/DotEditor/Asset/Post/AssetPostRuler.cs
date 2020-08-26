using DotEngine.Context;
using UnityEngine;

namespace DotEditor.Asset.Post
{
    public class AssetPostRuler : ScriptableObject
    {
        [ContextField(AssetPostContextKeys.CURRENT_ASSET_KEY,ContextUsage.In,false)]
        protected string assetPath;

        public virtual void Execute() 
        { }
    }
}
