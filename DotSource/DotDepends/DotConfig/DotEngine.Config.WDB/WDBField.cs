using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotEngine.Config.WDB
{
    public abstract class WDBField : IWDBValidationChecker
    {
        private const string NAME_REGEX_PATTERN = @"^[A-Za-z][A-Za-z0-9]{1,9}";

        public int Column { get; private set; }

        public string Type { get; private set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Platform { get; set; }
        public string DefaultContent { get; set; }
        public string Validation { get; set; }

        public abstract WDBFieldType FieldType { get; }

        public WDBFieldPlatform FieldPlatform
        {
            get
            {
                if (Enum.TryParse<WDBFieldPlatform>(Platform, true, out var fieldPlatform))
                {
                    return fieldPlatform;
                }
                return WDBFieldPlatform.All;
            }
            set
            {
                Platform = value.ToString().ToLower();
            }
        }

        public WDBField(int column, string type)
        {
            Column = column;
            Type = type;
        }

        public string GetContent()
        {
            return string.IsNullOrEmpty(DefaultContent) ? GetTypeDefaultContent() : DefaultContent;
        }

        public WDBCellValidation[] GetValidations()
        {
            List<WDBCellValidation> result = new List<WDBCellValidation>();

            List<string> rules = new List<string>();
            var defaultValidations = GetTypeDefaultValidations();
            if (defaultValidations != null && defaultValidations.Length > 0)
            {
                rules.AddRange(defaultValidations);
            }
            if (!string.IsNullOrEmpty(Validation))
            {
                var customValidations = (from rule in Validation.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries)
                                         select rule.Trim()).ToArray();
                if (customValidations != null && customValidations.Length > 0)
                {
                    rules.AddRange(customValidations);
                }
            }

            if (rules.Count > 0)
            {
                foreach (var rule in rules)
                {
                    if (WDBUtility.ParserValidationRule(rule, out var name, out var values))
                    {
                        WDBCellValidation validation = WDBCellValidationFactory.CreateValidation(name);
                        if (validation != null)
                        {
                            validation.SetRule(values);
                            result.Add(validation);
                        }
                        else
                        {
                            throw new WDBValidationNotFoundException(Column, rule);
                        }
                    }
                    else
                    {
                        throw new WDBValidationParserException(Column, rule);
                    }
                }
            }

            return result.ToArray();
        }

        protected abstract string GetTypeDefaultContent();
        protected abstract string[] GetTypeDefaultValidations();

        public void Check(WDBContext context)
        {
            if (string.IsNullOrEmpty(Name))
            {
                context.AppendError(WDBErrorMessages.FIELD_NAME_EMPTY_ERROR);
            }
            else if (!Regex.IsMatch(Name, NAME_REGEX_PATTERN))
            {
                context.AppendError(string.Format(WDBErrorMessages.FIELD_NAME_FORMAT_ERROR, Name, Column));
            }
        }

        public override string ToString()
        {
            return $"{{column = {Column},type = {Type},name = {Name}, desc = {Desc}, platform = {Platform}, default = {DefaultContent},validation = {Validation}}}";
        }
    }

    public static class WDBFieldFactory
    {
        private static Dictionary<string, Type> fieldTypeDic = new Dictionary<string, Type>();
        static WDBFieldFactory()
        {
            var assembly = typeof(WDBField).Assembly;
            var types = (from type in assembly.GetTypes()
                         where type.IsClass && !type.IsAbstract
                         let attrs = type.GetCustomAttributes(typeof(CustomFieldAttribute), false)
                         where attrs != null && attrs.Length > 0
                         let fieldType = (attrs[0] as CustomFieldAttribute).FieldType
                         select (fieldType, type)).ToArray();
            if (types != null && types.Length > 0)
            {
                foreach (var t in types)
                {
                    fieldTypeDic.Add(t.fieldType, t.type);
                }
            }
        }

        public static WDBField CreateField(int column, string fieldType)
        {
            if (fieldTypeDic.TryGetValue(fieldType, out var type))
            {
                return Activator.CreateInstance(type, new object[] { column, fieldType }) as WDBField;
            }
            return null;
        }
    }
}
