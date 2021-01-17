using System;
using System.IO;

namespace DotEditor.AssetChecker
{
    [MatchFilter("Match Filter/Folder", "In Parent Folder")]
    public class InParentFolderMatchFilter : MatchFilter
    {
        public bool IsIgnoreCase { get; set; } = true;
        public string ParentFolderName { get; set; } = null;

        public override bool IsMatch(string assetPath)
        {
            if(string.IsNullOrEmpty(ParentFolderName))
            {
                return false;
            }
            string assetDir = Path.GetDirectoryName(assetPath);
            string[] splitFolderNames = assetDir.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (splitFolderNames != null && splitFolderNames.Length > 0)
            {
                string parentFolderName = splitFolderNames[splitFolderNames.Length - 1];
                if(IsIgnoreCase)
                {
                    return parentFolderName.ToLower() == ParentFolderName.ToLower();
                }else
                {
                    return parentFolderName == ParentFolderName;
                }
            }

            return false;
        }
    }
}
