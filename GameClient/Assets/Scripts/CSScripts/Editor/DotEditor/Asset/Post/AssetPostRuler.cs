using UnityEngine;

namespace DotEditor.Asset.Post
{
    public class AssetPostRuler : ScriptableObject
    {
        public virtual void Execute(string assetPath) { }
    }
}
