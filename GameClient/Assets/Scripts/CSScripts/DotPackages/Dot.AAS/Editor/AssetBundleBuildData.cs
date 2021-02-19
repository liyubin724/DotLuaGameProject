using System;

namespace DotEditor.AAS
{
    [Serializable]
    public class AssetBundleBuildData
    {
        public string path = string.Empty;
        public string bundle = string.Empty;
        public string address = string.Empty;
        public string[] labels = new string[0];
    }
}
