using DotEditor.AAS.Matchers;
using DotEditor.Utilities;
using System;
using System.Collections.Generic;
using DotEngine.GUIExt.NativeDrawer;

namespace DotEditor.AAS.Filters
{
    [Serializable]
    public class AssetFilter : IAssetFilter
    {
        [OpenFolderPath(CanEditText =true)]
        public string assetFolder = string.Empty;
        public bool includeSubFolder = true;

        public ComposedMatcher matcher = new ComposedMatcher();

        public string[] Filter()
        {
            if(string.IsNullOrEmpty(assetFolder))
            {
                return new string[0];
            }

            string[] assetPaths = DirectoryUtility.GetAsset(assetFolder, includeSubFolder, true);
            if(assetPaths == null || assetPaths.Length == 0)
            {
                return new string[0];
            }
            List<string> result = new List<string>();
            foreach(var ap in assetPaths)
            {
                if(matcher.IsMatch(ap))
                {
                    result.Add(ap);
                }
            }
            return result.ToArray();
        }
    }
}
