using System;
using System.Collections.Generic;

namespace DotEditor.AssetChecker
{
    public class Operater : ICloneable
    {
        public bool enable = true;
        public List<IOperationRule> rules = new List<IOperationRule>();

        public void Add(IOperationRule rule)
        {
            if (rules == null)
            {
                rules = new List<IOperationRule>();
            }
            rules.Add(rule);
        }

        public void Remove(IOperationRule rule)
        {
            rules?.Remove(rule);
        }

        public void Clear()
        {
            rules.Clear();
        }

        public object Clone()
        {
            Operater operater = new Operater();
            operater.enable = enable;
            if (rules != null)
            {
                for (int i = 0; i < rules.Count; ++i)
                {
                    if (rules[i] != null)
                    {
                        operater.rules.Add((IOperationRule)rules[i].Clone());
                    }
                }
            }
            return operater;
        }

        public void Operate(string assetPath)
        {
            if (!enable || rules == null || rules.Count == 0)
            {
                return;
            }

            for (int i = 0; i < rules.Count; ++i)
            {
                if (rules[i] != null && rules[i].Enable)
                {
                    rules[i].Execute(assetPath);
                }
            }
        }
    }
}