using UnityObject = UnityEngine.Object;

namespace DotEditor.AssetChecker
{
    public interface IAnalyseRule
    {
        bool AnalyseAsset(UnityObject uObj, ref int errorCode);
    }
}
