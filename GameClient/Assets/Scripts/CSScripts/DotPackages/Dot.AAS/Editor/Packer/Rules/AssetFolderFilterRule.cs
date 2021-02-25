using DotEngine.GUIExt.NativeDrawer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DotEditor.AAS.Packer
{
    public enum AssetFolderFilterType
    {
        None = 0,
        InAnyFolder,
        InAnyParentFolder,
    }

    [Serializable]
    public class AssetFolderFilterRule
    {
        public AssetFolderFilterType filterType = AssetFolderFilterType.None;
        [VisibleIf("IsShowFolderNames")]
        public List<string> folderNames = new List<string>();

        public bool IsValid(string assetPath)
        {
            string fileDir = Path.GetDirectoryName(assetPath);
            List<string> folders = new List<string>(fileDir.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries));

            bool isValid = false;
            if(filterType == AssetFolderFilterType.None)
            {
                isValid = true;
            }else if(filterType == AssetFolderFilterType.InAnyFolder && folderNames.Count>0)
            {
                isValid = folderNames.Intersect(folders).Count() > 0;
            }else if(filterType == AssetFolderFilterType.InAnyParentFolder && folderNames.Count>0)
            {
                string parentFolder = folders[folders.Count - 1];
                return folderNames.IndexOf(parentFolder) >= 0;
            }

            return isValid;
        }

        private bool IsShowFolderNames() => filterType != AssetFolderFilterType.None;
    }
}
