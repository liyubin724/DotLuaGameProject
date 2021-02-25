using DotEditor.Utilities;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotEditor.AAS.Reprocessor
{
    public class PostprocessorHandle : AssetPostprocessor
    {
        private static string POSTPROCESS_FOLDER_NAME = "AAS";
        private static string FAILED_FILE_NAME = "postprocess-failed.txt";

        private static List<string> ignoreFileNameRegex = new List<string>()
        {
            @"\w*(.cs|.lua|.txt|.xml|.log|.dll|.md|.pdf)$",
            @"^~\w*",
        };

        private static string[] ReadFailedRecord()
        {
            string filePath = PathUtility.GetProjectDiskPath() + "/" + POSTPROCESS_FOLDER_NAME+"/"+FAILED_FILE_NAME;
            
            if(File.Exists(filePath))
            {
                string[] files = File.ReadAllLines(filePath);
                File.Delete(filePath);
                return files;
            }
            return new string[0];
        }

        private static void SaveFailedRecord(string[] assetPaths)
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
            string[] prefailedAssets = ReadFailedRecord();
            if (prefailedAssets != null && prefailedAssets.Length > 0)
            {
                allAssets.AddRange(prefailedAssets);
            }
            allAssets.AddRange(importedAssets);

            allAssets.RemoveAll((assetPath) =>
            {
                if (AssetDatabase.IsValidFolder(assetPath))
                {
                    return true;
                }

                string fileName = Path.GetFileName(assetPath).ToLower();
                if (ignoreFileNameRegex.Any((regex) =>
                {
                    return Regex.IsMatch(fileName, regex);
                }))
                {
                    return true;
                }
                return false;
            });

            AssetReprocessorSetting[] settings = AssetDatabaseUtility.FindInstances<AssetReprocessorSetting>();
            bool hasSetting = settings != null && settings.Length > 0;
            if(hasSetting)
            {
                for (int i = allAssets.Count - 1; i >= 0; --i)
                {
                    if (!settings[0].IsValid(allAssets[i]))
                    {
                        allAssets.RemoveAt(i);
                    }
                }
            }

            if(allAssets.Count>0)
            {
                if (!AssetReprocessUtility.ImportAssets(importedAssets, out var unknownAssetPaths))
                {
                    if(hasSetting) 
                    {
                        SaveFailedRecord(unknownAssetPaths);
                    }
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
