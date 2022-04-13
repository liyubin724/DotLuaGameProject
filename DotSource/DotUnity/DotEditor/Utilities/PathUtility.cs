using System.IO;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Utilities
{
    public static class PathUtility
    {
        public static string GetProjectDiskPath()
        {
            return Application.dataPath.Replace("/Assets", "");
        }

        public static bool IsAssetPath(string assetPath)
        {
            if(string.IsNullOrEmpty(assetPath) || !assetPath.StartsWith("Assets"))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 将相对磁盘的完整路径转换为相对于Assets的路径
        /// </summary>
        /// <param name="diskPath"></param>
        /// <returns></returns>
        public static string GetAssetPath(string diskPath)
        {
            diskPath = diskPath.Replace("\\", "/");
            if (diskPath.StartsWith(Application.dataPath))
            {
                return diskPath.Replace(Application.dataPath, "Assets");
            }
            return string.Empty;
        }

        public static string GetDiskPath(string assetPath)
        {
            if (string.IsNullOrEmpty(assetPath))
            {
                return string.Empty;
            }
            assetPath = assetPath.Replace("\\", "/");
            if (!assetPath.StartsWith("Assets"))
            {
                return string.Empty;
            }
            return Application.dataPath + assetPath.Substring(assetPath.IndexOf("Assets") + 6);
        }

        public static string GetSelectionAssetDirPath()
        {
            string path = "Assets";
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                if(obj == null)
                {
                    continue;
                }
                path = AssetDatabase.GetAssetPath(obj);
                if(Path.HasExtension(path))
                {
                    path = Path.GetDirectoryName(path);
                }
                break;
            }
            return path;
        }
    }
}


