using DotTool.ETD.Data;
using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DotTool.ETD.Fields
{
    public static class FieldTypeUtil
    {
        public static Type GetRealyType(FieldType fieldType)
        {
            FieldInfo fieldInfo = fieldType.GetType().GetField(fieldType.ToString());
            FieldRealyType realyType = fieldInfo.GetCustomAttribute<FieldRealyType>();
            return realyType?.RealyType;
        }

        public static bool IsNumberType(FieldType fieldType)
        {
            FieldRealyType realyTypeAttr = typeof(FieldType).GetField(fieldType.ToString()).GetCustomAttribute<FieldRealyType>();
            if (realyTypeAttr != null && realyTypeAttr.RealyType.IsValueType &&
                realyTypeAttr.RealyType.IsPrimitive)
            {
                return true;
            }
            return false;
        }

        public static bool IsStringType(FieldType fieldType)
        {
            FieldRealyType realyTypeAttr = typeof(FieldType).GetField(fieldType.ToString()).GetCustomAttribute<FieldRealyType>();
            if (realyTypeAttr != null && realyTypeAttr.RealyType == typeof(string))
            {
                return true;
            }
            return false;
        }

        private static FieldType StrToFieldType(string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr))
            {
                return FieldType.None;
            }

            string typeName = typeStr.Trim();
            if (string.IsNullOrEmpty(typeName))
            {
                return FieldType.None;
            }

            if (Enum.TryParse(typeName, true, out FieldType fieldType))
            {
                return fieldType;
            }
            else
            {
                return FieldType.None;
            }
        }

        private const string FIELD_TYPE_REGEX = @"^(?<typename>[A-Za-z]+)";
        public static FieldType GetFieldType(string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr))
            {
                return FieldType.None;
            }

            Match match = new Regex(FIELD_TYPE_REGEX).Match(typeStr);
            Group group = match.Groups["typename"];
            if (group != null && group.Success)
            {
                return StrToFieldType(group.Value);
            }
            return FieldType.None;
        }

        private const string REF_NAME_REGEX = @"^ref<(?<refname>[a-zA-Z]+[0-9]+)>$";
        public static string GetRefName(string typeStr)
        {
            if (string.IsNullOrEmpty(typeStr))
            {
                return string.Empty;
            }
            Match match = new Regex(REF_NAME_REGEX).Match(typeStr);
            Group group = match.Groups["refname"];
            if (group != null && group.Success)
            {
                return group.Value;
            }
            return string.Empty;
        }

        private const string ARRAY_INNER_REGEX = @"^list\[(?<innertype>[A-Za-z]+)[<]{0,1}(?<refname>[a-zA-Z]*[0-9]*)[>]{0,1}\]$";
        public static FieldType GetListInnerType(string typeStr, out string refName)
        {
            refName = string.Empty;

            if (string.IsNullOrEmpty(typeStr))
            {
                return FieldType.None;
            }
            Match match = new Regex(ARRAY_INNER_REGEX).Match(typeStr);
            Group innerTypeGroup = match.Groups["innertype"];
            if (innerTypeGroup != null && innerTypeGroup.Success)
            {
                Group refNameGroup = match.Groups["refname"];
                if (refNameGroup != null && refNameGroup.Success)
                {
                    refName = refNameGroup.Value;
                }
                return StrToFieldType(innerTypeGroup.Value);
            }
            return FieldType.None;
        }
    }
}
