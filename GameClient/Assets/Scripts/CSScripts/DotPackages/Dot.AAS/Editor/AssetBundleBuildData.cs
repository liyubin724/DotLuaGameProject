using System;
using System.Text;

namespace DotEditor.AAS
{
    [Serializable]
    public class AssetBundleBuildData
    {
        public string path = string.Empty;
        public string bundle = string.Empty;
        public string address = string.Empty;
        public string[] labels = new string[0];

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine($"path={path}");
            strBuilder.AppendLine($"bundle={bundle}");
            strBuilder.AppendLine($"address={address}");
            return strBuilder.ToString();
        }
    }
}
