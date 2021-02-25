using DotEngine.GUIExt.NativeDrawer;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DotEditor.AAS.Packer
{
    public enum AssetBundleAssignType
    {
        FullPath = 0,
        FullPathWithoutExt,
        FileName,
        FileNameWithoutExt,
        ParentFolderPath,
        
        Manual,
    }

    [Serializable]
    public class AssetBundleAssignRule
    {
        public AssetBundleAssignType assignType = AssetBundleAssignType.FullPath;
        [VisibleIf("IsShowBundleName")]
        public string bundleName = string.Empty;

        public string appendSuffix = string.Empty;

        public string GetBundleName(string assetPath)
        {
            string bName = string.Empty;

            string dirPath = Path.GetDirectoryName(assetPath).Replace("\\", "/");
            string fileName = ReplaceSpecialCharacter(Path.GetFileNameWithoutExtension(assetPath),"_");
            string fileExt = ReplaceSpecialCharacter(Path.GetExtension(assetPath),"_");
            if(assignType == AssetBundleAssignType.FullPath)
            {
                bName = $"{dirPath}/{fileName}{appendSuffix}{fileExt}";
            }else if(assignType == AssetBundleAssignType.FullPathWithoutExt)
            {
                bName = $"{dirPath}/{fileName}{appendSuffix}";
            }else if(assignType == AssetBundleAssignType.FileName)
            {
                bName = $"{fileName}{appendSuffix}{fileExt}";
            }else if(assignType == AssetBundleAssignType.FileNameWithoutExt)
            {
                bName = fileName+appendSuffix;
            }else if(assignType == AssetBundleAssignType.ParentFolderPath)
            {
                bName = dirPath;
            }else if(assignType == AssetBundleAssignType.Manual)
            {
                bName = bundleName + appendSuffix;
            }

            return bName.ToLower();
        }

        private string ReplaceSpecialCharacter(string str, string replaceStr)
        {
            return Regex.Replace(str, "[ \\[ \\] \\^ \\-*×――(^)|'$%~!@#$…&%￥—+=<>《》!！??？:：•`·、。，；,.;\"‘’“”-]", replaceStr);
        }

        private bool IsShowBundleName() => assignType == AssetBundleAssignType.Manual;
    }
}
