using DotEngine.GUIExt.NativeDrawer;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace DotEditor.AAS.Reprocessor
{
    [CreateAssetMenu(fileName = "asset_reprocess_setting", menuName = "AAS/Reprocess Setting")]
    [CustomDrawerEditor(IsShowBox = true)]
    public class AssetReprocessorSetting : ScriptableObject
    {
        public bool ignoreCase = true;
        public List<string> folders = new List<string>();

        public bool IsValid(string assetPath)
        {
            string assetFolder = Path.GetDirectoryName(assetPath).Replace("\\", "/");
            if (string.IsNullOrEmpty(assetFolder) || folders.Count == 0)
            {
                return false;
            }
            List<string> matchFolders = folders;
            if (ignoreCase)
            {
                assetFolder = assetFolder.ToLower();
                matchFolders = (from f in folders select f.ToLower()).ToList();
            }
            bool result = matchFolders.IndexOf(assetFolder) >= 0;
            if (!result)
            {
                result = matchFolders.Any((folder) =>
                {
                    return assetFolder.StartsWith(folder);
                });
            }
            return result;
        }

    }
}
