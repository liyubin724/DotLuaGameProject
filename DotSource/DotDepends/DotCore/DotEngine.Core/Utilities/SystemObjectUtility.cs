using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotEngine.Utilities
{
    public static class SystemObjectUtility
    {
        /// <summary>
        /// 将源数据中的属性或字段的值拷贝给指定的目标数据
        /// 只有属性或字段的名称及类型相同时才会进行拷贝操作
        /// </summary>
        /// <typeparam name="TFrom">源数据类型</typeparam>
        /// <typeparam name="TTo">目标数据类型</typeparam>
        /// <param name="from">源数据</param>
        /// <param name="to">目标数据</param>
        public static void ShallowCopy<TFrom, TTo>(TFrom from, TTo to)
        {
            ShallowCopyFields(from, to);
            ShallowCopyProperties(from, to);
        }

        /// <summary>
        /// 对属性进行浅拷贝
        /// 只有属性的名称及类型相同时才会进行拷贝操作
        /// </summary>
        /// <typeparam name="TFrom">源数据类型</typeparam>
        /// <typeparam name="TTo">目标数据类型</typeparam>
        /// <param name="from">源数据</param>
        /// <param name="to">目标数据</param>
        public static void ShallowCopyProperties<TFrom,TTo>(TFrom from,TTo to)
        {
            var fromProperties = from property in typeof(TFrom).GetProperties() where property.CanRead select property;
            var toProperties = from property in typeof(TTo).GetProperties() where property.CanWrite select property;

            foreach(var property in fromProperties)
            {
                var matchingProperty = FindMatchingProperty(toProperties, property);
                matchingProperty?.SetValue(to, matchingProperty.GetValue(from, null));
            }
        }

        private static PropertyInfo FindMatchingProperty(IEnumerable<PropertyInfo> toProperties,PropertyInfo fromProperty)
        {
            return toProperties.FirstOrDefault((property) => property.Name == fromProperty.Name && property.PropertyType == fromProperty.PropertyType);
        }

        /// <summary>
        /// 对字段进行浅拷贝
        /// 只有字段的名称及类型相同时才会进行拷贝操作
        /// </summary>
        /// <typeparam name="TFrom">源数据类型</typeparam>
        /// <typeparam name="TTo">目标数据类型</typeparam>
        /// <param name="from">源数据</param>
        /// <param name="to">目标数据</param>
        public static void ShallowCopyFields<TFrom,TTo>(TFrom from,TTo to)
        {
            var fromFields = from field in typeof(TFrom).GetFields() select field;
            var toFields = from field in typeof(TTo).GetFields() select field;
            foreach (var field in fromFields)
            {
                var matchingField = FindMatchingField(toFields, field);
                matchingField?.SetValue(to, matchingField.GetValue(from));
            }
        }

        private static FieldInfo FindMatchingField(IEnumerable<FieldInfo> toFields, FieldInfo fromField)
        {
            return toFields.FirstOrDefault((property) => property.Name == fromField.Name && property.FieldType == fromField.FieldType);
        }
    }
}
