using DotEngine.GUIExt.NativeDrawer;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DotEditor.AAS.Reprocessor
{
    [CreateAssetMenu(fileName = "asset_reprocess_setting", menuName = "AAS/Reprocess Setting")]
    [CustomDrawerEditor(IsShowBox = true)]
    public class AssetReprocessorSetting : ScriptableObject
    {
        public bool ignoreFolder = true;
        public List<string> targetFolders = new List<string>();
        public List<string> ignoreFileNameRegex = new List<string>();

        public bool IsValid(string assetPath)
        {
            string fileName = Path.GetFileName(assetPath).ToLower();
            if (ignoreFileNameRegex.Any((regex) =>
            {
                return Regex.IsMatch(fileName, regex);
            }))
            {
                return false;
            }

            string assetFolder = Path.GetDirectoryName(assetPath).Replace("\\", "/");
            if (string.IsNullOrEmpty(assetFolder) || targetFolders.Count == 0)
            {
                return false;
            }
            assetFolder = assetFolder.ToLower();
            List<string> matchFolders = (from f in targetFolders select f.ToLower()).ToList();
            if (matchFolders.IndexOf(assetFolder) >= 0 || matchFolders.Any((folder) =>
            {
                return assetFolder.StartsWith(folder);
            }))
            {
                return true;
            }
            return false;
        }

    }
}
