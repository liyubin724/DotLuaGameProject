using System;

namespace DotEngine.Context.Attributes
{
    public enum ContextUsage
    {
        Inject = 0,
        Extract,
        InjectAndExtract,
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ContextIEAttribute : Attribute
    {
        public object Key { get; private set; }
        public ContextUsage Usage { get; private set; }
        public bool Optional { get; private set; }

        public ContextIEAttribute(object key, ContextUsage usage = ContextUsage.Inject, bool isOption = true)
        {
            Key = key;
            Usage = usage;
            Optional = isOption;
        }
    }
}
