using DotEditor.AAS.Matchers;
using UnityEngine;

namespace DotEditor.AAS.Reprocessor
{
    public class AssetReprocessor : ScriptableObject, IAssetMatcher, IAssetReprocess
    {
        public ComposedMatcher matcher = new ComposedMatcher();
        public ComposedReprocess reprocessor = new ComposedReprocess();

        public void Execute(string assetPath)
        {
            reprocessor.Execute(assetPath);
        }

        public bool IsMatch(string assetPath)
        {
            return matcher.IsMatch(assetPath);
        }
    }
}
