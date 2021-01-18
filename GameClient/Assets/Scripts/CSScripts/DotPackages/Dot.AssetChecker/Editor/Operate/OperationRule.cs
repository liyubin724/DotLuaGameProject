using System;

namespace DotEditor.AssetChecker
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class OperatationRuleAttribute : Attribute
    {
        public string Menu { get; private set; }
        public string Label { get; private set; }
        public string MenuItemName
        {
            get
            {
                if(string.IsNullOrEmpty(Menu))
                {
                    return Label;
                }else
                {
                    return $"{Menu}/{Label}";
                }
            }
        }

        public OperatationRuleAttribute(string menu, string label)
        {
            Menu = menu;
            Label = label;
        }
    }

    public abstract class OperationRule : IOperationRule
    {
        public bool enable = true;
        public abstract void Execute(string assetPath);

        public object Clone()
        {
            OperationRule rule = (OperationRule)Activator.CreateInstance(GetType());
            rule.enable = enable;
            CloneTo(rule);

            return rule;
        }

        protected abstract void CloneTo(OperationRule rule);
    }
}
