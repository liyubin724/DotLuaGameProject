using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DotEditor.AAS.Matchers
{
    [Serializable]
    [CustomMatcherMenuAttribute("Matcher/Extension")]
    public class AssetExtensionMatcher : IAssetMatcher
    {
        public bool ingoreCase = true;
        public string extRegex = string.Empty;

        public bool IsMatch(string assetPath)
        {
            string assetExt = Path.GetExtension(assetPath);
            if(string.IsNullOrEmpty(assetExt) || string.IsNullOrEmpty(extRegex))
            {
                return false;
            }
            if(ingoreCase)
            {
                assetExt = assetExt.ToLower();
            }
            return Regex.IsMatch(assetExt, extRegex);
        }
    }
}
