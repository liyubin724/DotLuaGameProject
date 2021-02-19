using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DotEditor.AAS
{
    [Serializable]
    public class AssetNameFilterRule
    {
        public bool usedNameAsLowcase = false;
        public string nameFilter = string.Empty;

        public bool usedExtAsLowcase = false;
        public string extFilter = string.Empty;

        public bool IsValid(string assetPath)
        {
            string fileName = Path.GetFileName(assetPath);
            string fileExt = Path.GetExtension(assetPath);

            if (usedNameAsLowcase)
            {
                fileName = fileName.ToLower();
            }
            if (usedExtAsLowcase)
            {
                fileExt = fileExt.ToLower();
            }

            bool isValid = true;
            if (!string.IsNullOrEmpty(nameFilter))
            {
                isValid &= Regex.IsMatch(fileName, nameFilter);
            }
            if(!string.IsNullOrEmpty(extFilter))
            {
                isValid &= Regex.IsMatch(fileExt, extFilter);
            }
            
            return isValid;
        }
    }
}
