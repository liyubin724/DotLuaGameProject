using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotEngine.Injection
{
    internal class InjectReflectionTypeInfo
    {
        private List<InjectMemberInfo> injectMemberInfos = null;
        private List<InjectMemberInfo> extractMemberInfos = null;

        public Type TargetType
        {
            get; private set;
        }

        public InjectReflectionTypeInfo(Type type)
        {
            TargetType = type;
        }

        public void ReflectMemebers()
        {
            if (injectMemberInfos != null || extractMemberInfos != null)
            {
                return;
            }

            injectMemberInfos = new List<InjectMemberInfo>();
            extractMemberInfos = new List<InjectMemberInfo>();

            ReflectFields();
            ReflectProperties();
        }

        public void InjectTo<K>(IInjectContext<K> context,object target)
        {
            if(injectMemberInfos == null)
            {
                ReflectMemebers();
            }

            foreach(var mInfo in injectMemberInfos)
            {
                if(!context.TryToGet((K)mInfo.Key,out var value))
                {
                    if(!mInfo.IsOptional)
                    {
                        throw new InjectValueNotFoundException(mInfo.Key);
                    }
                    continue;
                }
                mInfo.SetValue(target, value);
            }
        }

        public void ExtractFrom<K>(IInjectContext<K> context,object target)
        {
            if(extractMemberInfos == null)
            {
                ReflectMemebers();
            }

            foreach(var mInfo in extractMemberInfos)
            {
                var value = mInfo.GetValue(target);
                if (!mInfo.IsOptional)
                {
                    context.AddOrUpdate((K)mInfo.Key, value);
                }else if (value != null)
                {
                    context.AddOrUpdate((K)mInfo.Key, value);
                }
            }
        }

        private void ReflectFields()
        {
            FieldInfo[] fieldInfos = TargetType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (fieldInfos != null && fieldInfos.Length > 0)
            {
                foreach (var fieldInfo in fieldInfos)
                {
                    InjectUsageAttribute attr = fieldInfo.GetCustomAttribute<InjectUsageAttribute>();
                    if (attr != null && attr.Key != null)
                    {
                        InjectMemberInfo imInfo = new InjectMemberInfo(fieldInfo, attr);
                        if (attr.OperationType == EInjectOperationType.Inject)
                        {
                            injectMemberInfos.Add(imInfo);
                        }
                        else
                        if (attr.OperationType == EInjectOperationType.Extract)
                        {
                            extractMemberInfos.Add(imInfo);
                        }
                        else
                        {
                            injectMemberInfos.Add(imInfo);
                            extractMemberInfos.Add(imInfo);
                        }
                    }
                }
            }
        }

        private void ReflectProperties()
        {
            PropertyInfo[] propertyInfos = TargetType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (propertyInfos != null && propertyInfos.Length > 0)
            {
                foreach (var propertyInfo in propertyInfos)
                {
                    InjectUsageAttribute attr = propertyInfo.GetCustomAttribute<InjectUsageAttribute>();
                    if (attr != null && attr.Key != null)
                    {
                        InjectMemberInfo imInfo = new InjectMemberInfo(propertyInfo, attr);
                        if (attr.OperationType == EInjectOperationType.Inject)
                        {
                            injectMemberInfos.Add(imInfo);
                        }
                        else
                        if (attr.OperationType == EInjectOperationType.Extract)
                        {
                            extractMemberInfos.Add(imInfo);
                        }
                        else
                        {
                            injectMemberInfos.Add(imInfo);
                            extractMemberInfos.Add(imInfo);
                        }
                    }
                }
            }
        }
    }
}
