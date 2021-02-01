using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotEngine.Context
{
    public static class ContextUtil
    {
        private static Dictionary<Type, Dictionary<ContextUsage, List<FieldInfo>>> cachedTypeFieldDic = new Dictionary<Type, Dictionary<ContextUsage, List<FieldInfo>>>();
        private static Dictionary<Type, Dictionary<ContextUsage, List<PropertyInfo>>> cachedTypePropertyDic = new Dictionary<Type, Dictionary<ContextUsage, List<PropertyInfo>>>();

        private static void CachedFields(Type type)
        {
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

        private static void CachedProperties(Type type)
        {
            List<PropertyInfo> inProperties = new List<PropertyInfo>();
            List<PropertyInfo> outProperties = new List<PropertyInfo>();

            Dictionary<ContextUsage, List<PropertyInfo>> propertyDic = new Dictionary<ContextUsage, List<PropertyInfo>>();
            propertyDic.Add(ContextUsage.In, inProperties);
            propertyDic.Add(ContextUsage.Out, outProperties);

            cachedTypePropertyDic.Add(type, propertyDic);

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var property in properties)
            {
                var attr = property.GetCustomAttribute<ContextFieldAttribute>();
                if (attr == null)
                {
                    continue;
                }

                if (attr.Usage != ContextUsage.In && property.CanRead)
                {
                    outProperties.Add(property);
                }
                if (attr.Usage != ContextUsage.Out && property.CanWrite)
                {
                    inProperties.Add(property);
                }
            }
        }

        private static void InjectFields<K>(IEnvContext<K> context, Type injectObjType,object injectObj)
        {
            List<FieldInfo> fields = cachedTypeFieldDic[injectObjType][ContextUsage.In];
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

        private static void InjectProperties<K>(IEnvContext<K> context, Type injectObjType,object injectObj)
        {
            List<PropertyInfo> properties = cachedTypePropertyDic[injectObjType][ContextUsage.Out];
            if(properties!=null && properties.Count>0)
            {
                foreach(var property in properties)
                {
                    var attr = property.GetCustomAttribute<ContextFieldAttribute>();

                    if (!context.TryGet((K)attr.Key, out object fieldValue))
                    {
                        if (fieldValue == null && !attr.Optional)
                        {
                            throw new Exception($"ContextUtil::Inject->Data not found.fieldName = {property.Name},key = {attr.Key}");
                        }
                    }
                    property.SetValue(injectObj, fieldValue);
                }
            }
        }

        public static void Inject<K>(IEnvContext<K> context, object injectObj)
        {
            if (injectObj == null || context == null)
            {
                throw new ArgumentNullException("ContextUtil::Inject->argument is null.");
            }
            Type injectObjType = injectObj.GetType();

            if(!cachedTypeFieldDic.ContainsKey(injectObjType))
            {
                CachedFields(injectObjType);
            }

            if(!cachedTypePropertyDic.ContainsKey(injectObjType))
            {
                CachedProperties(injectObjType);
            }

            InjectFields(context, injectObjType, injectObj);
            InjectProperties(context, injectObjType, injectObj);
        }

        private static void ExtractFields<K>(IEnvContext<K> context, Type extractObjType, object extractObj)
        {
            List<FieldInfo> fields = cachedTypeFieldDic[extractObjType][ContextUsage.Out];
            if (fields != null && fields.Count > 0)
            {
                foreach (var field in fields)
                {
                    if (typeof(IEnvContext<>).IsAssignableFrom(field.FieldType))
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

        private static void ExtractProperties<K>(IEnvContext<K> context, Type extractObjType, object extractObj)
        {
            List<PropertyInfo> properties = cachedTypePropertyDic[extractObjType][ContextUsage.Out];
            if (properties != null && properties.Count > 0)
            {
                foreach (var property in properties)
                {
                    if (typeof(IEnvContext<>).IsAssignableFrom(property.PropertyType))
                    {
                        throw new InvalidOperationException("Context can only be used with the InjectUsage.In option.");
                    }

                    var attr = property.GetCustomAttribute<ContextFieldAttribute>();

                    object fieldValue = property.GetValue(extractObj);
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

        public static void Extract<K>(IEnvContext<K> context, object extractObj)
        {
            if (extractObj == null || context == null)
            {
                throw new ArgumentNullException("ContextUtil::Extract->argument is null");
            }

            Type extractObjType = extractObj.GetType();

            if (!cachedTypeFieldDic.ContainsKey(extractObjType))
            {
                CachedFields(extractObjType);
            }

            if (!cachedTypePropertyDic.ContainsKey(extractObjType))
            {
                CachedProperties(extractObjType);
            }

            ExtractFields(context, extractObjType, extractObj);
            ExtractProperties(context, extractObjType, extractObj);
        }
    }
}
