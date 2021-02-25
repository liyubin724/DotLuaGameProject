using DotEditor.Utilities;
using DotEngine.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DotEditor.AAS.Reprocessor
{
    public static class AssetReprocessUtility
    {
        private static Type[] reprocessTypes = null;
        public static Type[] GetReprocessTypes()
        {
            if(reprocessTypes == null)
            {
                reprocessTypes = AssemblyUtility.GetDerivedTypes(typeof(IAssetReprocess));
            }
            return reprocessTypes;
        }

        public static void ShowMenu(Action<IAssetReprocess> createCallback)
        {
            GenericMenu menu = new GenericMenu();
            Type[] types = GetReprocessTypes();
            foreach (var type in types)
            {
                var attrs = type.GetCustomAttributes(typeof(CustomReprocessMenuAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    CustomReprocessMenuAttribute attr = attrs[0] as CustomReprocessMenuAttribute;
                    menu.AddItem(new GUIContent(attr.MenuPath), false, () =>
                    {
                        IAssetReprocess reprocess = Activator.CreateInstance(type) as IAssetReprocess;
                        createCallback?.Invoke(reprocess);
                    });
                }
            }
            menu.ShowAsContext();
        }


        private static string POSTPROCESS_FOLDER_NAME = "AAS";
        private static string FAILED_FILE_NAME = "postprocess-failed.txt";
        private static string[] ReadFailedRecord()
        {
            string filePath = PathUtility.GetProjectDiskPath() + "/" + POSTPROCESS_FOLDER_NAME + "/" + FAILED_FILE_NAME;

            if (File.Exists(filePath))
            {
                string[] files = File.ReadAllLines(filePath);
                File.Delete(filePath);
                return files;
            }
            return new string[0];
        }

        private static void SaveFailedRecord(string[] assetPaths)
        {
            if (assetPaths != null && assetPaths.Length > 0)
            {
                string folderPath = PathUtility.GetProjectDiskPath() + "/" + POSTPROCESS_FOLDER_NAME;
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string filePath = folderPath + "/" + FAILED_FILE_NAME;
                File.WriteAllLines(filePath, assetPaths);
            }
        }

        private static List<string> ignoreFileNameRegex = new List<string>()
        {
            @"\w*(.cs|.lua|.txt|.xml|.log|.dll|.md|.pdf|.bin)$",
            @"^~\w*",
        };
        public static string[] PostprocessImportAssets(string[] importedAssets)
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
            if (hasSetting)
            {
                allAssets.RemoveAll((assetPath) =>
                {
                    return !settings.Any((setting) =>
                    {
                        return setting.IsValid(assetPath);
                    });
                });
            }

            if (allAssets.Count > 0)
            {
                if (!ImportAssets(importedAssets, out var unknownAssetPaths))
                {
                    if (hasSetting)
                    {
                        SaveFailedRecord(unknownAssetPaths);
                    }
                    return unknownAssetPaths;
                }
            }

            return null;
        }

        private static bool ImportAssets(string[] assetPaths,out string[] unknownAssetPaths)
        {
            List<string> results = new List<string>();
            AssetReprocessor[] reprocessors = AssetDatabaseUtility.FindInstances<AssetReprocessor>();
            if (reprocessors != null && reprocessors.Length > 0)
            {
                foreach(var assetPath in assetPaths)
                {
                    bool isReprocess = false;
                    foreach (var reprocessor in reprocessors)
                    {
                        if (reprocessor.IsMatch(assetPath))
                        {
                            reprocessor.Execute(assetPath);
                            isReprocess = true;
                            break;
                        }
                    }
                    if(!isReprocess)
                    {
                        results.Add(assetPath);
                    }
                }
            }
            unknownAssetPaths = results.ToArray();
            return results.Count == 0;
        }

        [MenuItem("Game/Assets/AAS/Reprocess All")]
        public static void StartReprocessor()
        {
            AssetReprocessorSetting[] settings = AssetDatabaseUtility.FindInstances<AssetReprocessorSetting>();
            List<string> files = new List<string>();
            foreach(var setting in settings)
            {
                setting.validFolders.ForEach((folder) =>
                {
                    files.AddRange(DirectoryUtility.GetAsset(folder, true, true));
                });
            }
            files = files.Distinct().ToList();

            string[] failedAssets = PostprocessImportAssets(files.ToArray());
            if(failedAssets!=null)
            {
                EditorUtility.DisplayDialog("Error", "", "OK");
                Debug.LogError(string.Join("\n", failedAssets));
            }
        }
    }
}
