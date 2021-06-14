using DotTool.ETD.Data;
using DotTool.ETD.Util;
using System;

namespace DotTool.ETD.Fields
{
    public static class FieldFactory
    {
        public static Field GetField(
            int col,
            string name,
            string desc,
            string type,
            string platform,
            string defaultValue,
            string validation)
        {
            FieldType fieldType = FieldTypeUtil.GetFieldType(type);

            Type resultType;
            if (fieldType != FieldType.None)
            {
                string fieldName = fieldType.ToString() + "Field";
                resultType = AssemblyUtil.GetTypeByName(fieldName,true);
            }
            else
            {
                resultType = typeof(ErrorField);
            }

            return (Field)Activator.CreateInstance(resultType, col, name, desc, type, platform, defaultValue, validation);
        }
    }
}
