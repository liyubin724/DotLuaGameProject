using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DotEngine.Config.WDB
{
    public static class WDBUtility
    {
        private static Dictionary<WDBFieldType, Type> fieldDic = null;

        private static void FindFields()
        {
            fieldDic = new Dictionary<WDBFieldType, Type>();

            Assembly assembly = typeof(WDBField).Assembly;
            foreach (var type in assembly.GetTypes())
            {
                if (type.IsSubclassOf(typeof(WDBField)))
                {
                    var attr = type.GetCustomAttribute<WDBFieldLinkAttribute>();
                    if (attr != null)
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
            if (fieldDic == null)
            {
                FindFields();
            }

            WDBField field;
            if (fieldDic.TryGetValue(fieldType, out var fieldClass))
            {
                field = (WDBField)Activator.CreateInstance(fieldClass, col);
            }
            else
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
                         where type.IsSubclassOf(typeof(WDBCellValidation))
                         select type).ToArray();
            foreach (var type in types)
            {
                validationDic.Add(type.Name.ToLower(), type);
            }
        }

        //用于查找校验规则
        //比如：maxlen解析后 name : maxlen 
        //            maxlen(1,2,ffff)匹配后:name:maxlen , params:1,2,ffff
        private const string VALIDATION_RULE_PATTERN = @"(^(?<name>[A-Za-z]{1,})$|^(?<name>[A-Za-z]{1,})[\s]*\((?<params>\S*)\))";
        public static WDBCellValidation[] CreateValidation(string[] validationRules)
        {
            if (validationDic == null)
            {
                FindValidations();
            }
            if (validationRules == null || validationRules.Length == 0)
            {
                return null;
            }
            WDBCellValidation[] validations = new WDBCellValidation[validationRules.Length];
            for (int i = 0; i < validations.Length; i++)
            {
                string rule = validationRules[i];
                Match nameMatch = new Regex(VALIDATION_RULE_PATTERN).Match(rule);
                Group nameGroup = nameMatch.Groups["name"];
                Group paramsGroup = nameMatch.Groups["params"];

                string ruleName = nameGroup.Success ? nameGroup.Value.Trim() : null;
                string[] ruleParams = new string[0];
                if (paramsGroup.Success && !string.IsNullOrEmpty(paramsGroup.Value))
                {
                    ruleParams = paramsGroup.Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                }
                if (!string.IsNullOrEmpty(ruleName) && validationDic.TryGetValue($"{ruleName.ToLower()}validation", out var type))
                {
                    WDBCellValidation validation = (WDBCellValidation)Activator.CreateInstance(type);
                    validation.SetRule(rule, ruleParams);
                    validations[i] = validation;
                }
                else
                {
                    ErrorValidation validation = new ErrorValidation();
                    validation.SetRule(rule, null);
                    validations[i] = validation;
                }
            }

            return validations;
        }
    }
}
