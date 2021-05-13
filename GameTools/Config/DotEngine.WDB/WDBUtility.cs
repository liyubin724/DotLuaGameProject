using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DotEngine.WDB
{

    public static class WDBUtility
    {
        private static Dictionary<WDBFieldType, Type> fieldDic = null;

        private static void FindFields()
        {
            fieldDic = new Dictionary<WDBFieldType, Type>();

            Assembly assembly = typeof(WDBField).Assembly;
            foreach(var type in assembly.GetTypes())
            {
                if(type.IsSubclassOf(typeof(WDBField)))
                {
                    var attr = type.GetCustomAttribute<WDBFieldLinkAttribute>();
                    if(attr!=null)
                    {
                        fieldDic.Add(attr.FieldType, type);
                    }
                }
            }
        }

        public static WDBField CreateField(int col, string name, string desc, string type, string platform, string defaultValue, string validationRule)
        {
            WDBFieldType fieldType = WDBFieldType.None;
            if (Enum.TryParse<WDBFieldType>(type, true, out var ft))
            {
                fieldType = ft;
            }
            if(fieldDic == null)
            {
                FindFields();
            }

            WDBField field;
            if (fieldDic.TryGetValue(fieldType, out var fieldClass))
            {
                field = (WDBField)Activator.CreateInstance(fieldClass, col);
            }else
            {
                field = new WDBField(col);
            }
            field.Name = name;
            field.Desc = desc;
            field.Type = type;
            field.Platform = platform;
            field.DefaultValue = defaultValue;
            field.ValidationRule = validationRule;

            return field;
        }

        private static Dictionary<string, Type> validationDic = null;
        private static void FindValidations()
        {
            validationDic = new Dictionary<string, Type>();

            var types = (from assembly in AppDomain.CurrentDomain.GetAssemblies()
                         from type in assembly.GetTypes()
                         where type.IsSubclassOf(typeof(WDBValidation))
                         select type).ToArray();
            foreach(var type in types)
            {
                validationDic.Add(type.Name.ToLower(), type);
            }
        }

        public static WDBValidation[] CreateValidation(string[] validationRules)
        {
            if(validationDic == null)
            {
                FindValidations();
            }
            if(validationRules == null || validationRules.Length == 0)
            {
                return null;
            }
            WDBValidation[] validations = new WDBValidation[validationRules.Length];
            for(int i =0;i<validations.Length;i++)
            {
                string rule = validationRules[i];
                Match nameMatch = new Regex(@"(?<name>[a-zA-Z]*)").Match(rule);
                Group nameGroup = nameMatch.Groups["name"];
                if(nameGroup!=null)
                {
                    string ruleName = nameGroup.Value;
                    if(!string.IsNullOrEmpty(ruleName) && validationDic.TryGetValue($"{ruleName.ToLower()}validation", out var type))
                    {


                        WDBValidation validation = (WDBValidation)(Activator.CreateInstance(type, rule));
                        validations[i] = validation;
                        continue;
                    }
                }else
                {
                    validations[i] = null;
                }
            }

            return validations;
        }
    }
}
