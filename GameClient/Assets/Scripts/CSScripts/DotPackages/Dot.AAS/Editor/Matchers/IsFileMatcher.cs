using DotEditor.Utilities;
using System;
using System.IO;

namespace DotEditor.AAS.Matchers
{
    [Serializable]
    [CustomMatcherMenuAttribute("Matcher/Is File")]
    public class IsFileMatcher : IAssetMatcher
    {
        public bool IsMatch(string assetPath)
        {
            string diskPath = PathUtility.GetDiskPath(assetPath);
            return File.Exists(diskPath);
        }
    }
}
