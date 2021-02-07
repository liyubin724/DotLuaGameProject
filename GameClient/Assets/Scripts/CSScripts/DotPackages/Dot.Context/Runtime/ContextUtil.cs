using DotEngine.Context.Attributes;
using DotEngine.Context.Interfaces;
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
            List<FieldInfo> injectFields = new List<FieldInfo>();
            List<FieldInfo> extractFields = new List<FieldInfo>();

            Dictionary<ContextUsage, List<FieldInfo>> fieldDic = new Dictionary<ContextUsage, List<FieldInfo>>();
            fieldDic.Add(ContextUsage.Inject, injectFields);
            fieldDic.Add(ContextUsage.Extract, extractFields);

            cachedTypeFieldDic.Add(type, fieldDic);

            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var field in fields)
            {
                var attr = field.GetCustomAttribute<ContextIEAttribute>();
                if (attr == null)
                {
                    continue;
                }

                if (attr.Usage != ContextUsage.Inject)
                {
                    extractFields.Add(field);
                }
                if (attr.Usage != ContextUsage.Extract)
                {
                    injectFields.Add(field);
                }
            }
        }

        private static void CachedProperties(Type type)
        {
            List<PropertyInfo> injectProperties = new List<PropertyInfo>();
            List<PropertyInfo> extractProperties = new List<PropertyInfo>();

            Dictionary<ContextUsage, List<PropertyInfo>> propertyDic = new Dictionary<ContextUsage, List<PropertyInfo>>();
            propertyDic.Add(ContextUsage.Inject, injectProperties);
            propertyDic.Add(ContextUsage.Extract, extractProperties);

            cachedTypePropertyDic.Add(type, propertyDic);

            PropertyInfo[] properties = type.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var property in properties)
            {
                var attr = property.GetCustomAttribute<ContextIEAttribute>();
                if (attr == null)
                {
                    continue;
                }

                if (attr.Usage != ContextUsage.Inject && property.CanRead)
                {
                    extractProperties.Add(property);
                }
                if (attr.Usage != ContextUsage.Extract && property.CanWrite)
                {
                    injectProperties.Add(property);
                }
            }
        }



        private static void InjectFields<K>(IContextContainer<K> context, IContextObject injectObj)
        {
            List<FieldInfo> fields = cachedTypeFieldDic[injectObj.GetType()][ContextUsage.Inject];
            if (fields != null && fields.Count > 0)
            {
                foreach (var field in fields)
                {
                    var attr = field.GetCustomAttribute<ContextIEAttribute>();

                    if (!context.TryGet((K)attr.Key, out object value))
                    {
                        if (value == null && !attr.Optional)
                        {
                            throw new ContextValueNullException(attr.Key);
                        }
                    }
                    field.SetValue(injectObj, value);
                }
            }
        }

        private static void InjectProperties<K>(IContextContainer<K> context, IContextObject injectObj)
        {
            List<PropertyInfo> properties = cachedTypePropertyDic[injectObj.GetType()][ContextUsage.Inject];
            if(properties!=null && properties.Count>0)
            {
                foreach(var property in properties)
                {
                    var attr = property.GetCustomAttribute<ContextIEAttribute>();

                    if (!context.TryGet((K)attr.Key, out object value))
                    {
                        if (value == null && !attr.Optional)
                        {
                            throw new ContextValueNullException(attr.Key);
                        }
                    }
                    property.SetValue(injectObj, value);
                }
            }
        }

        public static void Inject<K>(IContextContainer<K> context, IContextObject injectObj)
        {
            if (injectObj == null || context == null)
            {
                throw new ArgumentNullException("argument is null.");
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

            InjectFields(context, injectObj);
            InjectProperties(context, injectObj);
        }

        private static void ExtractFields<K>(IContextContainer<K> context, IContextObject extractObj)
        {
            List<FieldInfo> fields = cachedTypeFieldDic[extractObj.GetType()][ContextUsage.Extract];
            if (fields != null && fields.Count > 0)
            {
                foreach (var field in fields)
                {
                    if (typeof(IContextContainer<>).IsAssignableFrom(field.FieldType))
                    {
                        throw new InvalidOperationException("Context can only be used with the InjectUsage.In option.");
                    }

                    var attr = field.GetCustomAttribute<ContextIEAttribute>();
                    object value = field.GetValue(extractObj);
                    if (!attr.Optional)
                    {
                        context.AddOrUpdate((K)attr.Key, value);
                    }
                    else if (value != null)
                    {
                        context.AddOrUpdate((K)attr.Key, value);
                    }
                }
            }
        }

        private static void ExtractProperties<K>(IContextContainer<K> context, IContextObject extractObj)
        {
            List<PropertyInfo> properties = cachedTypePropertyDic[extractObj.GetType()][ContextUsage.Extract];
            if (properties != null && properties.Count > 0)
            {
                foreach (var property in properties)
                {
                    if (typeof(IContextContainer<>).IsAssignableFrom(property.PropertyType))
                    {
                        throw new InvalidOperationException("Context can only be used with the InjectUsage.In option.");
                    }

                    var attr = property.GetCustomAttribute<ContextIEAttribute>();
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

        public static void Extract<K>(IContextContainer<K> context, IContextObject extractObj)
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

            ExtractFields(context, extractObj);
            ExtractProperties(context, extractObj);
        }
    }
}
