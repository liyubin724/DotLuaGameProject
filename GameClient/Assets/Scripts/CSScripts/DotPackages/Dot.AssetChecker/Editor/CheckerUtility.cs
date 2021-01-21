using DotEditor.Utilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters;

namespace DotEditor.AssetChecker
{
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

        static CheckerUtility()
        {
            CHECKER_CONFIG_DIR = $"{PathUtility.GetProjectDiskPath()}/{CHECKER_ROOT_DIR}/{CHECKER_CONFIG_DIR}";
            CHECKER_RESULT_INFO_PATH = $"{PathUtility.GetProjectDiskPath()}/{CHECKER_ROOT_DIR}/{CHECKER_RESULT_INFO_PATH}";
        }

        private static CheckerResultInfo checkerResultInfo = null;
        public static CheckerResultInfo ReadCheckerResultInfo()
        {
            if(checkerResultInfo == null)
            {
                if (File.Exists(CHECKER_RESULT_INFO_PATH))
                {
                    string content = File.ReadAllText(CHECKER_RESULT_INFO_PATH);
                    checkerResultInfo = JsonConvert.DeserializeObject<CheckerResultInfo>(content, new JsonSerializerSettings()
                    {
                        TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                        TypeNameHandling = TypeNameHandling.All,
                    });
                }else
                {
                    checkerResultInfo = new CheckerResultInfo();
                }
            }

            return checkerResultInfo;           
        }

        public static void SaveCheckerResultInfo()
        {
            if (!Directory.Exists(CHECKER_CONFIG_DIR))
            {
                Directory.CreateDirectory(CHECKER_CONFIG_DIR);
            }

            string jsonContent = JsonConvert.SerializeObject(checkerResultInfo, Formatting.Indented, new JsonSerializerSettings()
            {
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                TypeNameHandling = TypeNameHandling.All,
            });
            File.WriteAllText(CHECKER_RESULT_INFO_PATH, jsonContent);
        }

        private static List<CheckerFileInfo> checkerFileInfos = null;
        public static List<CheckerFileInfo> ReadCheckerFileInfos()
        {
            if(checkerFileInfos != null)
            {
                return checkerFileInfos;
            }

            checkerFileInfos = new List<CheckerFileInfo>();
            if (!Directory.Exists(CHECKER_CONFIG_DIR))
            {
                return checkerFileInfos;
            }

            string[] files = Directory.GetFiles(CHECKER_CONFIG_DIR, "*.json", SearchOption.TopDirectoryOnly);
            if (files != null && files.Length > 0)
            {
                foreach (var file in files)
                {
                    Checker checker = JsonConvert.DeserializeObject<Checker>(File.ReadAllText(file), new JsonSerializerSettings()
                    {
                        TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                        TypeNameHandling = TypeNameHandling.All,
                    });
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

            return checkerFileInfos;
        }

        public static void SaveCheckerFileInfo(CheckerFileInfo checkerFileInfo)
        {
            if (!Directory.Exists(CHECKER_CONFIG_DIR))
            {
                Directory.CreateDirectory(CHECKER_CONFIG_DIR);
            }

            string jsonContent = JsonConvert.SerializeObject(checkerFileInfo.checker,Formatting.Indented, new JsonSerializerSettings()
            {
                TypeNameAssemblyFormat = FormatterAssemblyStyle.Simple,
                TypeNameHandling = TypeNameHandling.All,
            });
            File.WriteAllText(checkerFileInfo.filePath, jsonContent);

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
