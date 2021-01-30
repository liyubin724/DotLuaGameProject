using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotEditor.Utilities
{
    public static class DirectoryUtility
    {
        /// <summary>
        /// 查找指定的目标下资源
        /// </summary>
        /// <param name="assetDir">基于Assets的目录</param>
        /// <param name="isIncludeSubfolder">是否包括子目录</param>
        /// <returns></returns>
        public static string[] GetAsset(string assetDir, bool isIncludeSubfolder,bool isIgnoreMeta)
        {
            string diskDir = PathUtility.GetDiskPath(assetDir);
            string[] files = Directory.GetFiles(diskDir, "*.*", isIncludeSubfolder ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            if(files!=null && files.Length>0)
            {
                return (
                        from file in files 
                        where !isIgnoreMeta ||( isIgnoreMeta && Path.GetExtension(file).ToLower() != ".meta")
                        select PathUtility.GetAssetPath(file.Replace("\\", "/"))
                        ).ToArray();
            }
            return null;
        }

        /// <summary>
        /// 查找指定的目标中匹配指定的格式的文件（使用文件名与后缀进行匹配）
        /// </summary>
        /// <example>
        /// string[] assetPaths = DirectoryUtil.GetAsset("Assets/UI", true, @"^\w*.prefab$");
        /// </example>
        /// <param name="assetDir">基于Assets的目录</param>
        /// <param name="isIncludeSubfolder">是否包括子目录</param>
        /// <param name="filePattern">文件名匹配正则表达式</param>
        /// <returns></returns>
        public static string[] GetAsset(string assetDir,bool isIncludeSubfolder,string filePattern)
        {
            string[] assetPaths = GetAsset(assetDir, isIncludeSubfolder, false);
            if(assetPaths == null || assetPaths.Length==0)
            {
                return null;
            }

            return (
                from assetPath in assetPaths 
                where Regex.IsMatch(Path.GetFileName(assetPath), filePattern)
                select assetPath
                ).ToArray();
        }

        /// <summary>
        /// 从sourceDirName目录中复制所有的目录及文件到指定的destDirName目录中
        /// 可以通过ignoreFileExt参数指定忽略的文件后缀。默认采用小写进行比对，忽略大小写
        /// string[] ignoreFileExt = new string[]{".meta"}
        /// </summary>
        /// <param name="sourceDirName">源目录</param>
        /// <param name="destDirName">目标目录</param>
        /// <param name="ignoreFileExt">忽略文件后缀</param>
        private static void Copy(string sourceDirName, string destDirName, string[] ignoreFileExt = null)
        {
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            foreach (string folderPath in Directory.GetDirectories(sourceDirName, "*", SearchOption.AllDirectories))
            {
                if (!Directory.Exists(folderPath.Replace(sourceDirName, destDirName)))
                    Directory.CreateDirectory(folderPath.Replace(sourceDirName, destDirName));
            }

            foreach (string filePath in Directory.GetFiles(sourceDirName, "*.*", SearchOption.AllDirectories))
            {
                var fileDirName = Path.GetDirectoryName(filePath).Replace("\\", "/");

                var fileExt = Path.GetExtension(filePath).ToLower();
                if(ignoreFileExt != null && Array.IndexOf(ignoreFileExt,fileExt)>=0)
                {
                    continue;
                }

                var fileName = Path.GetFileName(filePath);
                string newFilePath = Path.Combine(fileDirName.Replace(sourceDirName, destDirName), fileName);

                File.Copy(filePath, newFilePath, true);
            }
        }
    }
}
