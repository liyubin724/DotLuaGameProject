using System;

namespace DotEngine.BD.Datas
{
    [AttributeUsage(AttributeTargets.Class,AllowMultiple =false,Inherited =false)]
    public class ConditionDataAttribute : Attribute
    {
        public string MenuPrefix { get; private set; }
        public string Label { get; private set; }

        public ConditionDataAttribute(string menu, string label)
        {
            MenuPrefix = menu;
            Label = label;
        }
    }
}
