using DotEngine.Context.Attributes;
using DotEngine.Context.Interfaces;
using UnityEngine;

namespace DotEditor.Assets.Post
{
    public class AssetPostRuler : ScriptableObject,IContextObject
    {
        [ContextIE(AssetPostContextKeys.ASSET_FILTER_RESULT_KEY, ContextUsage.Inject, false)]
        protected string[] assetPaths;

        public virtual void Execute() 
        { }
    }
}
