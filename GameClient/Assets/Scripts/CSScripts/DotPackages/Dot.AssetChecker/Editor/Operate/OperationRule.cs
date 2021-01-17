using System;

namespace DotEditor.AssetChecker
{
    public abstract class OperationRule : IOperationRule
    {
        public bool enable = true;
        public bool Enable => enable;

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
