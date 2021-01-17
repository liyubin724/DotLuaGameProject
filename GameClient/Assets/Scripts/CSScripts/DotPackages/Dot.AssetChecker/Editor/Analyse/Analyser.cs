using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace DotEditor.AssetChecker
{
    public class Analyser
    {
        public bool enable = true;
        public List<IAnalyseRule> rulers = null;

        public void Add(IAnalyseRule ruler)
        {
            if(rulers == null)
            {
                rulers = new List<IAnalyseRule>();
            }
            rulers.Add(ruler);
        }

        public void Remove(IAnalyseRule ruler)
        {
            rulers?.Remove(ruler);
        }

        public void Clear()
        {
            rulers?.Clear();
        }

        public bool Analyse(string assetPath, ref int errorCode)
        {
            if (!enable || rulers == null || rulers.Count==0)
            {
                return true;
            }

            if (string.IsNullOrEmpty(assetPath))
            {
                errorCode = ResultCode.ERR_ANALYSE_ASSET_PATH_EMPTY;
                return false;
            }

            string fullPath = "";
            if (!File.Exists(fullPath))
            {
                errorCode = ResultCode.ERR_ANALYSE_ASSET_NOT_FOUND;
                return false;
            }

            UnityObject uObj = AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityObject));
            if (uObj == null)
            {
                errorCode = ResultCode.ERR_ANALYSE_ASSET_NOT_UNITYOBJECT;
                return false;
            }

            foreach (var ruler in rulers)
            {
                if (ruler != null && !ruler.AnalyseAsset(uObj, ref errorCode))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
