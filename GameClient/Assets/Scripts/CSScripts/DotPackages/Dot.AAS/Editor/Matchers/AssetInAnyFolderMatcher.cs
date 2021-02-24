using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotEditor.AAS.Matchers
{
    [Serializable]
    [CustomMatcherMenu("In Any Folder")]
    public class AssetInAnyFolderMatcher : IAssetMatcher
    {
        public bool ignoreCase = true;
        public List<string> folders = new List<string>();

        public bool IsMatch(string assetPath)
        {
            string assetFolder = Path.GetDirectoryName(assetPath).Replace("\\", "/");
            if(string.IsNullOrEmpty(assetFolder) || folders.Count == 0)
            {
                return false;
            }
            List<string> matchFolders = folders;
            if(ignoreCase)
            {
                assetFolder = assetFolder.ToLower();
                matchFolders = (from f in folders select f.ToLower()).ToList();
            }
            return matchFolders.IndexOf(assetFolder) >= 0;
        }
    }
}
