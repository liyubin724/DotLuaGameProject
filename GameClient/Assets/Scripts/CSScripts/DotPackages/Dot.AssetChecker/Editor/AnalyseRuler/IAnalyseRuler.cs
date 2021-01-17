using UnityObject = UnityEngine.Object;

namespace DotEditor.AssetChecker
{
    public enum AnalyseRulerCategory
    {
        None = 0,
        Texture,
    }

    public interface IAnalyseRuler
    {
        bool Enable { get; set; }

        bool AnalyseAsset(UnityObject uObj, ref int errorCode);
    }
}
