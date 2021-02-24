using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DotEditor.AAS.Matchers
{
    [Serializable]
    [CustomMatcherMenu("Name")]
    public class AssetNameMatcher : IAssetMatcher
    {
        public bool ignoreCase = true;
        public string nameRegex = string.Empty;

        public bool IsMatch(string assetPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            if(string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(nameRegex))
            {
                return false;
            }

            if(ignoreCase)
            {
                fileName = fileName.ToLower();
            }
            return Regex.IsMatch(fileName, nameRegex);
        }
    }
}
