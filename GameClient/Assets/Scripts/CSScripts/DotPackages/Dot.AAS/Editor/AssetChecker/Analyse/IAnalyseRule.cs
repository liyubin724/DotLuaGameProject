using Newtonsoft.Json;
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
        bool Enable { get; }
    }

    public interface IFileAnalyseRule : IAnalyseRule
    {
        bool AnalyseAsset(string assetPath, ref int errorCode);
    }

    public interface IUnityObjectAnalyseRule : IAnalyseRule
    {
        bool AnalyseAsset(UnityObject uObj, ref int errorCode);
    }

    public abstract class FileAnalyseRule : IFileAnalyseRule
    {
        public bool enable = true;

        [JsonIgnore]
        public bool Enable => enable;

        public abstract bool AnalyseAsset(string assetPath, ref int errorCode);
    }

    public abstract class UnityObjectAnalyseRule : IUnityObjectAnalyseRule
    {
        public bool enable = true;

        [JsonIgnore]
        public bool Enable => enable;

        public abstract bool AnalyseAsset(UnityObject uObj, ref int errorCode);
    }
}
