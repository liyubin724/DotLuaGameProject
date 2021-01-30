using System;

namespace DotEditor.Asset.Dependency
{
    [Serializable]
    public class AssetDependencyData
    {
        public string assetPath;
        public string[] directlyDepends = new string[0];
        public string[] allDepends = new string[0];
    }
}
