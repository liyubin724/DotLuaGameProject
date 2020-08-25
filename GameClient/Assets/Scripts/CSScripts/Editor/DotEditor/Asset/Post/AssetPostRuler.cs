using DotEngine.Context;
using UnityEngine;

namespace DotEditor.Asset.Post
{

    public class AssetPostRuler : ScriptableObject
    {
        public virtual void Execute(StringContext context, string assetPath) 
        { }
    }
}
