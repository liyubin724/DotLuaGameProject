using System;

namespace DotEngine.Injection
{
    public enum EInjectOperationType
    {
        Inject = 0,
        Extract,
        InjectAndExtract,
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class InjectUsageAttribute : Attribute
    {
        public object Key { get; private set; }
        public EInjectOperationType OperationType { get; private set; }
        public bool IsOptional { get; private set; }

        public InjectUsageAttribute(object key, EInjectOperationType operationType = EInjectOperationType.Inject, bool isOptional = true)
        {
            Key = key;
            OperationType = operationType;
            IsOptional = isOptional;
        }
    }
}
