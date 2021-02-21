using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace DotEditor.AssetChecker
{
    public class CheckerPostprocessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            List<CheckerFileInfo> checkerFiles = CheckerUtility.ReadCheckerFileInfos();
            CheckerResultInfo resultInfo = CheckerUtility.ReadCheckerResultInfo();

            if (importedAssets!=null && importedAssets.Length>0)
            {
                foreach(var assetPath in importedAssets)
                {
                    if(AssetDatabase.IsValidFolder(assetPath) && !CheckerUtility.ReadSetting().isFolderAsAsset)
                    {
                        continue;
                    }

                    CheckerFileInfo cfi = checkerFiles.FirstOrDefault((cFileInfo) =>
                    {
                        return cFileInfo.checker.enable && cFileInfo.checker.IsMatch(assetPath);
                    });

                    if(cfi == null)
                    {
                        resultInfo.AddPassedResult(assetPath);
                    }else
                    {
                        Checker checker = cfi.checker;
                        int errorCode = 0;
                        if (checker.DoAnalyse(assetPath, ref errorCode))
                        {
                            checker.DoOperate(assetPath);

                            resultInfo.RemoveAsset(assetPath);
                        }
                        else
                        {
                            resultInfo.AddFailedResult(assetPath, errorCode);
                        }
                    }
                }
            }

            if(deletedAssets!=null && deletedAssets.Length>0)
            {
                foreach(var assetPath in deletedAssets)
                {
                    resultInfo.RemoveAsset(assetPath);
                }
            }

            CheckerUtility.SaveCheckerResultInfo();
        }
    }
}
