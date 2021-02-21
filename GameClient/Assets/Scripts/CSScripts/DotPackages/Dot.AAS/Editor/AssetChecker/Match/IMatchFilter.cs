using System;
using Newtonsoft.Json;

namespace DotEditor.AssetChecker
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class MatchFilterAttribute : Attribute
    {
        public string Menu { get; private set; }
        public string Label { get; private set; }
        public string MenuItemName
        {
            get
            {
                if (string.IsNullOrEmpty(Menu))
                {
                    return Label;
                }
                else
                {
                    return $"{Menu}/{Label}";
                }
            }
        }

        public MatchFilterAttribute(string menu, string label)
        {
            Menu = menu;
            Label = label;
        }
    }

    public interface IMatchFilter : ICloneable
    {
        bool Enable { get; }
        bool IsMatch(string assetPath);
    }

    public abstract class MatchFilter : IMatchFilter
    {
        public bool enable = true;

        [JsonIgnore]
        public bool Enable => enable;

        public bool IsMatch(string assetPath)
        {
            if (!enable)
            {
                return false;
            }
            if (string.IsNullOrEmpty(assetPath))
            {
                return false;
            }
            return MatchAsset(assetPath);
        }

        protected abstract bool MatchAsset(string assetPath);

        public object Clone()
        {
            MatchFilter filter = (MatchFilter)Activator.CreateInstance(GetType());
            filter.enable = enable;
            CloneTo(filter);

            return filter;
        }

        protected abstract void CloneTo(MatchFilter filter);
    }

}