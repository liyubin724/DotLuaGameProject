using System;

namespace DotEditor.AssetChecker
{
    public abstract class MatchFilter : IMatchFilter
    {
        public bool enable = true;

        public object Clone()
        {
            MatchFilter filter = (MatchFilter)Activator.CreateInstance(GetType());
            filter.enable = enable;
            CloneTo(filter);

            return filter;
        }

        protected abstract void CloneTo(MatchFilter filter);

        public bool IsMatch(string assetPath)
        {
            if(!enable)
            {
                return false;
            }
            if(string.IsNullOrEmpty(assetPath))
            {
                return false;
            }
            return MatchAsset(assetPath);
        }

        protected abstract bool MatchAsset(string assetPath);
    }
}