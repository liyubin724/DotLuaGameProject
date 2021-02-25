using System;
using System.IO;

namespace DotEditor.AAS.Packer
{
    public enum AssetAddressAssignType
    {
        FullPath = 0,
        FullPathWithoutExt,
        FileName,
        FileNameWithoutExt,
    }

    [Serializable]
    public class AssetAddressAssignRule
    {
        public AssetAddressAssignType assignType = AssetAddressAssignType.FileName;

        public string GetAddress(string assetPath)
        {
            string dirPath = Path.GetDirectoryName(assetPath).Replace("\\","/");
            string fileName = Path.GetFileName(assetPath);
            string fileNameWithoutExt = Path.GetFileNameWithoutExtension(assetPath);

            if(assignType == AssetAddressAssignType.FullPathWithoutExt)
            {
                return $"{dirPath}/{fileNameWithoutExt}";
            }else if(assignType == AssetAddressAssignType.FileName)
            {
                return fileName;
            }else if(assignType == AssetAddressAssignType.FileNameWithoutExt)
            {
                return fileNameWithoutExt;
            }else
            {
                return assetPath;
            }
        }
    }
}
