using System;

namespace DotEngine.Context.Attributes
{
    public enum ContextUsage
    {
        InjectAndExtract = 0,
        Inject,
        Extract,
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ContextIEAttribute : Attribute
    {
        public object Key { get; set; }
        public ContextUsage Usage { get; set; }
        public bool Optional { get; set; }

        public ContextIEAttribute(object key,ContextUsage usage = ContextUsage.Inject, bool isOption = true)
        {
            Key = key;
            Usage = usage;
            Optional = isOption;
        }
    }
}
