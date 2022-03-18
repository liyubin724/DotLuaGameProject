using System;
using System.Reflection;

namespace DotEngine.Injection
{
    internal class InjectMemberInfo
    {
        private FieldInfo fieldInfo;
        private PropertyInfo propertyInfo;
        private InjectUsageAttribute usageAttribute;

        public string ValueName { get; private set; }
        public Type ValueType { get; private set; }

        public object Key
        {
            get
            {
                return usageAttribute.Key;
            }
        }

        public bool IsOptional
        {
            get
            {
                return usageAttribute.IsOptional;
            }
        }
        
        public InjectMemberInfo(FieldInfo fInfo,InjectUsageAttribute attr)
        {
            fieldInfo = fInfo;
            ValueName = fieldInfo.Name;
            ValueType = fieldInfo.FieldType;
            usageAttribute = attr;
        }

        public InjectMemberInfo(PropertyInfo pInfo,InjectUsageAttribute attr)
        {
            propertyInfo = pInfo;
            ValueName = propertyInfo.Name;
            ValueType = propertyInfo.PropertyType;
            usageAttribute = attr;
        }

        public object GetValue(object target)
        {
            if (fieldInfo != null)
            {
                return fieldInfo.GetValue(target);
            }
            return propertyInfo.GetValue(target);
        }

        public void SetValue(object target,object value)
        {
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(target, value);
            }else if(propertyInfo!=null && propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(target, value);
            }
        }
    }
}
