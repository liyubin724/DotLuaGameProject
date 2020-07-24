using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotEngine.Context
{
    public static class ContextUtil
    {
        private static Dictionary<Type, Dictionary<ContextUsage, List<FieldInfo>>> cachedTypeFieldDic = new Dictionary<Type, Dictionary<ContextUsage, List<FieldInfo>>>();

        private static void CacheFields(Type type)
        {
            if (cachedTypeFieldDic.ContainsKey(type))
            {
                return;
            }

            List<FieldInfo> inFields = new List<FieldInfo>();
            List<FieldInfo> outFields = new List<FieldInfo>();

            Dictionary<ContextUsage, List<FieldInfo>> fieldDic = new Dictionary<ContextUsage, List<FieldInfo>>();
            fieldDic.Add(ContextUsage.In, inFields);
            fieldDic.Add(ContextUsage.Out, outFields);

            cachedTypeFieldDic.Add(type, fieldDic);

            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute<ContextFieldAttribute>();
                if (attr == null)
                {
                    continue;
                }
                if (attr.Usage != ContextUsage.In)
                {
                    outFields.Add(field);
                }
                if (attr.Usage != ContextUsage.Out)
                {
                    inFields.Add(field);
                }
            }
        }

        public static List<FieldInfo> GetFields(Type type, ContextUsage usage)
        {
            if (!cachedTypeFieldDic.ContainsKey(type))
            {
                CacheFields(type);
            }

            if (cachedTypeFieldDic.TryGetValue(type, out Dictionary<ContextUsage, List<FieldInfo>> usageFieldDic))
            {
                if (usageFieldDic.TryGetValue(usage, out List<FieldInfo> fields))
                {
                    return fields;
                }
            }
            return null;
        }

        public static void Inject<K>(IEnvContext<K> context, object injectObj)
        {
            if (injectObj == null || context == null)
            {
                throw new ArgumentNullException("ContextUtil::Inject->argument is null.");
            }
            List<FieldInfo> fields = GetFields(injectObj.GetType(), ContextUsage.In);
            if (fields != null && fields.Count > 0)
            {
                foreach (var field in fields)
                {
                    var attr = field.GetCustomAttribute<ContextFieldAttribute>();

                    if (!context.TryGet((K)attr.Key, out object fieldValue))
                    {
                        if (fieldValue == null && !attr.Optional)
                        {
                            throw new Exception($"ContextUtil::Inject->Data not found.fieldName = {field.Name},key = {attr.Key}");
                        }
                    }
                    field.SetValue(injectObj, fieldValue);
                }
            }
        }

        public static void Extract<K>(IEnvContext<K> context, object extractObj)
        {
            if (extractObj == null || context == null)
            {
                throw new ArgumentNullException("ContextUtil::Extract->argument is null");
            }

            List<FieldInfo> fields = GetFields(extractObj.GetType(), ContextUsage.Out);
            if (fields != null && fields.Count > 0)
            {
                foreach (var field in fields)
                {
                    if(typeof(IEnvContext<>).IsAssignableFrom(field.FieldType))
                    {
                        throw new InvalidOperationException("Context can only be used with the InjectUsage.In option.");
                    }

                    var attr = field.GetCustomAttribute<ContextFieldAttribute>();

                    object fieldValue = field.GetValue(extractObj);
                    if (!attr.Optional)
                    {
                        context.AddOrUpdate((K)attr.Key, fieldValue);
                    }
                    else if (fieldValue != null)
                    {
                        context.AddOrUpdate((K)attr.Key, fieldValue);
                    }
                }
            }
        }
    }
}
