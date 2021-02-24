using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotEditor.AAS.Matchers
{
    [Serializable]
    [CustomMatcherMenu("In Any Parent Folder")]
    public class AssetInAnyParentFolderMatcher : IAssetMatcher
    {
        public bool ignoreCase = true;
        public List<string> parentFolders = new List<string>();

        public bool IsMatch(string assetPath)
        {
            string assetFolder = Path.GetDirectoryName(assetPath).Replace("\\", "/");
            if (string.IsNullOrEmpty(assetFolder) || parentFolders.Count == 0)
            {
                return false;
            }

            List<string> assetFolders = new List<string>(assetFolder.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries));
            List<string> matchFolders = parentFolders;
            if(ignoreCase)
            {
                assetFolders = (from f in assetFolders select f.ToLower()).ToList();
                matchFolders = (from f in matchFolders select f.ToLower()).ToList();
            }

            return matchFolders.Except(assetFolders).Count() > 0;
        }
    }
}
