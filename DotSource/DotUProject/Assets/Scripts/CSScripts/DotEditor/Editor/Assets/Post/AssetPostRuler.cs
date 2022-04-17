using DotEngine.Injection;
using UnityEngine;

namespace DotEditor.Assets.Post
{
    public class AssetPostRuler : ScriptableObject
    {
        [InjectUsage(AssetPostContextKeys.ASSET_FILTER_RESULT_KEY, EInjectOperationType.Inject, false)]
        protected string[] assetPaths;

        public virtual void Execute() 
        { }
    }
}
