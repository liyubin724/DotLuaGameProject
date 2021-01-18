using System;

namespace DotEditor.AssetChecker
{
    public interface IOperationRule : ICloneable
    {
        void Execute(string assetPath);
    }
}
