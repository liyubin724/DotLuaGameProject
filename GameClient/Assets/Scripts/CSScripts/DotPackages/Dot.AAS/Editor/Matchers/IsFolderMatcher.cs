using DotEditor.Utilities;
using System.IO;
using System;

namespace DotEditor.AAS.Matchers
{
    [Serializable]
    [CustomMatcherMenuAttribute("Matcher/Is Folder")]
    public class IsFolderMatcher : IAssetMatcher
    {
        public bool IsMatch(string assetPath)
        {
            string diskPath = PathUtility.GetDiskPath(assetPath);
            return Directory.Exists(diskPath);
        }
    }
}
