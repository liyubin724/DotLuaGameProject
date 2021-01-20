using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using DotEditor.Utilities;

namespace DotEditor.AssetChecker
{
    public class CheckerPostprocessor : AssetPostprocessor
    {
        public static string CHECKER_CONFIG_DIR = "Tools/AssetChecker";
        public static string CHECKER_FAILED_ASSET_PATH = "asset_checker_failed.txt";
        public static string CHECKER_PASSED_ASSET_PATH = "asset_checker_passed.txt";

        static CheckerPostprocessor()
        {
            CHECKER_CONFIG_DIR = $"{PathUtility.GetProjectDiskPath()}/{CHECKER_CONFIG_DIR}";
            CHECKER_FAILED_ASSET_PATH = $"{CHECKER_CONFIG_DIR}/{CHECKER_FAILED_ASSET_PATH}";
            CHECKER_PASSED_ASSET_PATH = $"{CHECKER_CONFIG_DIR}/{CHECKER_PASSED_ASSET_PATH}";
        }

        private static List<Checker> checkers = new List<Checker>();
        private static List<string> failedAssetList = new List<string>();
        private static List<string> passedAssetList = new List<string>();
        [InitializeOnLoadMethod]
        public static void LoadCheckers()
        {
            checkers.Clear();
            if(!Directory.Exists(CHECKER_CONFIG_DIR))
            {
                if(!Directory.CreateDirectory(CHECKER_CONFIG_DIR).Exists)
                {
                    return;
                }    
            }

            if(File.Exists(CHECKER_FAILED_ASSET_PATH))
            {
                failedAssetList.AddRange(File.ReadAllText(CHECKER_FAILED_ASSET_PATH).Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
            }
            if (File.Exists(CHECKER_PASSED_ASSET_PATH))
            {
                passedAssetList.AddRange(File.ReadAllText(CHECKER_PASSED_ASSET_PATH).Split(new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
            }

            string[] files = Directory.GetFiles(CHECKER_CONFIG_DIR, "*.json", SearchOption.TopDirectoryOnly);
            if(files!=null && files.Length>0)
            {
                foreach(var file in files)
                {
                    Checker checker = JsonConvert.DeserializeObject<Checker>(File.ReadAllText(file));
                    if(checker!=null)
                    {
                        checkers.Add(checker);
                    }
                }
            }
        }

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if(importedAssets!=null && importedAssets.Length>0)
            {
                foreach(var assetPath in importedAssets)
                {
                    bool isFailed = false;
                    bool isFound = false;
                    foreach(var checker in checkers)
                    {
                        if(checker.enable && checker.IsMatch(assetPath))
                        {
                            isFound = true;
                            //if(checker.DoAnalyse(assetPath ref int errorCode))
                            //{

                            //}
                        }
                    }
                }
            }
        }
    }
}
