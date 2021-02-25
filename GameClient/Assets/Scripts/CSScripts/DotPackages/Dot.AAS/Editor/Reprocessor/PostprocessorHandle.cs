using DotEditor.Utilities;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AAS.Reprocessor
{
    public class PostprocessorHandle : AssetPostprocessor
    {
        private static string POSTPROCESS_FOLDER_NAME = "AAS";

        private static string FAILED_FILE_NAME = "postprocess-failed.txt";
        private static string[] ReadFailedAssets()
        {
            string filePath = PathUtility.GetProjectDiskPath() + "/" + POSTPROCESS_FOLDER_NAME+"/"+FAILED_FILE_NAME;
            
            if(File.Exists(filePath))
            {
                return File.ReadAllLines(filePath);
            }
            return new string[0];
        }

        private static void SaveFailedAssets(string[] assetPaths)
        {
            if(assetPaths!=null && assetPaths.Length>0)
            {
                string folderPath = PathUtility.GetProjectDiskPath() + "/" + POSTPROCESS_FOLDER_NAME;
                if(!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = folderPath + "/" + FAILED_FILE_NAME;
                File.WriteAllLines(filePath, assetPaths);
            }
        }

        private static void OnPostprocessImportAssets(string[] importedAssets)
        {
            List<string> allAssets = new List<string>();
            string[] prefailedAssets = ReadFailedAssets();
            if (prefailedAssets != null && prefailedAssets.Length > 0)
            {
                allAssets.AddRange(prefailedAssets);
            }
            foreach (var asset in importedAssets)
            {
                if (!AssetDatabase.IsValidFolder(asset))
                {
                    allAssets.Add(asset);
                }
            }

            AssetReprocessorSetting[] settings = AssetDatabaseUtility.FindInstances<AssetReprocessorSetting>();
            if(settings!=null && settings.Length>0)
            {
                for (int i = allAssets.Count - 1; i >= 0; --i)
                {
                    if(!settings[0].IsValid(allAssets[i]))
                    {
                        allAssets.RemoveAt(i);
                    }
                }
            }

            if(allAssets.Count>0)
            {
                if (!AssetReprocessUtility.ImportAssets(importedAssets, out var unknownAssetPaths))
                {
                    SaveFailedAssets(unknownAssetPaths);
                    if(EditorUtility.DisplayDialog("Error","Some assets can't be processed!!","OK"))
                    {
                        Debug.LogError(string.Join(",", unknownAssetPaths));
                    }
                }
                else
                {
                    Debug.Log("Postprocess success");
                }
            }
        }

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if(importedAssets!=null && importedAssets.Length>0)
            {
                OnPostprocessImportAssets(importedAssets);
            }
        }
    }
}
