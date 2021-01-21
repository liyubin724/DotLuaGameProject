using DotEditor.Utilities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

using JsonUtility = DotEngine.Utilities.JsonUtility;

namespace DotEditor.AssetChecker
{
    public class CheckerSetting
    {
        public bool isFolderAsAsset = false;
    }

    public class CheckerFileInfo
    {
        public string filePath;
        public Checker checker;
    }

    public class CheckerResultInfo
    {
        public List<FailedResultInfo> failedResults = new List<FailedResultInfo>();
        public List<PassedResultInfo> passedResults = new List<PassedResultInfo>();

        public void AddFailedResult(string assetPath, int errorCode)
        {
            var result = failedResults.FirstOrDefault((r) =>
            {
                return r.assetPath == assetPath;
            });
            if (result != null)
            {
                result.errorCode = errorCode;
            }
            else
            {
                failedResults.Add(new FailedResultInfo()
                {
                    assetPath = assetPath,
                    errorCode = errorCode
                });
            }
        }

        public void AddPassedResult(string assetPath)
        {
            var hasResult = passedResults.Any((r) =>
            {
                return r.assetPath == assetPath;
            });
            if (hasResult)
            {
                return;
            }
            passedResults.Add(new PassedResultInfo()
            {
                assetPath = assetPath
            });
        }

        public void RemoveAsset(string assetPath)
        {
            for (int i = failedResults.Count - 1; i >= 0; --i)
            {
                if (failedResults[i].assetPath == assetPath)
                {
                    failedResults.RemoveAt(i);
                    break;
                }
            }

            for (int i = passedResults.Count - 1; i >= 0; --i)
            {
                if (passedResults[i].assetPath == assetPath)
                {
                    passedResults.RemoveAt(i);
                    break;
                }
            }
        }

        public class FailedResultInfo
        {
            public string assetPath;
            public int errorCode;
        }

        public class PassedResultInfo
        {
            public string assetPath;
        }
    }

    internal static class CheckerUtility
    {
        public static string CHECKER_ROOT_DIR = "Tools";
        public static string CHECKER_CONFIG_DIR = "AssetChecker";
        public static string CHECKER_RESULT_INFO_PATH = "asset_checker_result.txt";
        public static string CHECKER_SETTING_PATH = "asset_checker_setting.txt";

        static CheckerUtility()
        {
            CHECKER_CONFIG_DIR = $"{PathUtility.GetProjectDiskPath()}/{CHECKER_ROOT_DIR}/{CHECKER_CONFIG_DIR}";
            CHECKER_RESULT_INFO_PATH = $"{PathUtility.GetProjectDiskPath()}/{CHECKER_ROOT_DIR}/{CHECKER_RESULT_INFO_PATH}";
            CHECKER_SETTING_PATH = $"{PathUtility.GetProjectDiskPath()}/{CHECKER_ROOT_DIR}/{CHECKER_SETTING_PATH}";
        }

        private static void WriteAsJson<T>(string filePath, T data) where T:class,new()
        {
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                if(!Directory.CreateDirectory(dir).Exists)
                {
                    Debug.LogError("CheckerUtility::SaveAsJson->Create dir failed.dir = " + dir);
                    return;
                }
            }
            File.WriteAllText(filePath, JsonUtility.ToJsonWithType(data));
        }

        private static T ReadFromJson<T>(string filePath,bool createIfNot = true) where T : class, new()
        {
            T data = null;
            if(File.Exists(filePath))
            {
                data = (T)JsonUtility.FromJsonWithType(File.ReadAllText(filePath));
            }
            if(data == null && createIfNot)
            {
                data = new T();
            }
            return data;
        }

        public static CheckerSetting setting = null;
        public static CheckerSetting ReadSetting()
        {
            if(setting == null)
            {
                setting = ReadFromJson<CheckerSetting>(CHECKER_SETTING_PATH);
            }
            return setting;
        }

        public static void WriteSetting()
        {
            WriteAsJson<CheckerSetting>(CHECKER_SETTING_PATH, setting);
        }

        private static CheckerResultInfo checkerResultInfo = null;
        public static CheckerResultInfo ReadCheckerResultInfo()
        {
            if(checkerResultInfo == null)
            {
                checkerResultInfo = ReadFromJson<CheckerResultInfo>(CHECKER_RESULT_INFO_PATH);
            }

            return checkerResultInfo;           
        }

        public static void SaveCheckerResultInfo()
        {
            WriteAsJson<CheckerResultInfo>(CHECKER_RESULT_INFO_PATH,checkerResultInfo);
        }

        private static List<CheckerFileInfo> checkerFileInfos = null;
        public static List<CheckerFileInfo> ReadCheckerFileInfos()
        {
            if(checkerFileInfos == null)
            {
                checkerFileInfos = new List<CheckerFileInfo>();

                if(Directory.Exists(CHECKER_CONFIG_DIR))
                {
                    string[] files = Directory.GetFiles(CHECKER_CONFIG_DIR, "*.json", SearchOption.TopDirectoryOnly);
                    if (files != null && files.Length > 0)
                    {
                        foreach (var file in files)
                        {
                            Checker checker = ReadFromJson<Checker>(file,false);
                            if (checker != null)
                            {
                                checkerFileInfos.Add(new CheckerFileInfo()
                                {
                                    filePath = file.Replace("\\", "/"),
                                    checker = checker,
                                });
                            }
                        }
                    }
                }
            }
            return checkerFileInfos;
        }

        public static void SaveCheckerFileInfo(CheckerFileInfo checkerFileInfo)
        {
            WriteAsJson<Checker>(checkerFileInfo.filePath, checkerFileInfo.checker);

            CheckerFileInfo cachedCFI = checkerFileInfos.FirstOrDefault((cfi) =>
            {
                return cfi.filePath == checkerFileInfo.filePath;
            });

            if(cachedCFI!=null)
            {
                checkerFileInfos.Remove(cachedCFI);
            }
            checkerFileInfos.Add(checkerFileInfo);
        }

        public static void DeleteCheckerFileInfo(CheckerFileInfo cfi)
        {
            checkerFileInfos.Remove(cfi);
            File.Delete(cfi.filePath);
        }
    }
}
