using System;

namespace DotEditor.AssetChecker
{
    public class MatchFilterAttribute : Attribute
    {
        public string Menu { get; private set; }
        public string Label { get; private set; }

        public MatchFilterAttribute(string menu,string label)
        {
            Menu = menu;
            Label = label;
        }
    }
}
