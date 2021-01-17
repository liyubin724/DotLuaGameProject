using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace DotEditor.AssetChecker
{
    public abstract class AnalyseSets
    {
        public bool Enable { get; set; } = true;
        public List<IAnalyseRuler> Rulers { get; } = new List<IAnalyseRuler>();

        public void Add(IAnalyseRuler ruler)
        {
            Rulers.Add(ruler);
        }

        public void Remove(IAnalyseRuler ruler)
        {
            Rulers.Remove(ruler);
        }

        public void Clear()
        {
            Rulers.Clear();
        }

        public bool AnalyseAsset(string assetPath,ref int errorCode)
        {
            if(!Enable)
            {
                return true;
            }

            if(Rulers.Count==0)
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

            foreach(var ruler in Rulers)
            {
                if(ruler != null && !ruler.AnalyseAsset(uObj, ref errorCode))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
