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
        
        public bool isMainAsset = false;
        public bool isNeedPreload = false;
        public bool isNeverDestroy = false;

        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine($"path={path}");
            strBuilder.AppendLine($"bundle={bundle}");
            if(isMainAsset)
            {
                strBuilder.AppendLine($"address={address}");
                strBuilder.AppendLine($"labels={((labels!=null&&labels.Length>0)?string.Join(",",labels):"")}");
            }
            return strBuilder.ToString();
        }
    }
}
