namespace DotEditor.AssetChecker
{
    public interface IOperationRuler
    {
        bool Enable { get; set; }
        bool Run(string assetPath,ref int errorCode);
    }
}
