using System;
using UnityObject = UnityEngine.Object;

namespace DotEditor.AssetChecker
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AnalyseRuleAttribute:Attribute
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

        public AnalyseRuleAttribute(string menu,string label)
        {
            Menu = menu;
            Label = label;
        }
    }

    public interface IAnalyseRule
    {
    }

    public interface IFileAnalyseRule
    {
        bool AnalyseAsset(string assetPath, ref int errorCode);
    }

    public interface IUnityObjectAnalyseRule
    {
        bool AnalyseAsset(UnityObject uObj, ref int errorCode);
    }


}
