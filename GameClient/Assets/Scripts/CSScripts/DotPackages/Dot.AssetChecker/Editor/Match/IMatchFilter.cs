using System;

namespace DotEditor.AssetChecker
{
    public interface IMatchFilter : ICloneable
    {
        bool IsMatch(string assetPath);
    }
}