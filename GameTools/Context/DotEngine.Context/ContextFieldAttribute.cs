using System;

namespace DotEngine.Context
{
    public enum ContextUsage
    {
        InOut = 0,
        In,
        Out,
    }

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ContextFieldAttribute : Attribute
    {
        public object Key { get; set; }
        public ContextUsage Usage { get; set; }
        public bool Optional { get; set; }

        public ContextFieldAttribute(object key) : this(key,ContextUsage.In,true)
        {
        }

        public ContextFieldAttribute(object key,ContextUsage usage) : this(key, usage, true)
        {

        }

        public ContextFieldAttribute(object key,ContextUsage usage,bool isOption)
        {
            Key = key;
            Usage = usage;
            Optional = isOption;
        }
    }
}
