using System;

namespace DotEditor.AssetChecker
{
    public interface IOperationRule : ICloneable
    {
        bool Enable { get; }
        void Execute(string assetPath);
    }
}
