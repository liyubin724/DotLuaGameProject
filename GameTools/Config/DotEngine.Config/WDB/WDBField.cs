using System;
using System.Collections.Generic;

namespace DotEngine.Config.WDB
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class WDBFieldLinkAttribute : Attribute
    {
        public WDBFieldType FieldType { get; private set; }
        public WDBFieldLinkAttribute(WDBFieldType fieldType)
        {
            FieldType = fieldType;
        }
    }

    public enum WDBFieldType
    {
        None = 0,

        Bool,

        Int,
        Id,
        Ref,

        Long,

        Float,

        String,
        Text,
        Address,
        Lua,
    }

    public enum WDBFieldPlatform
    {
        All = 0,

        Client,
        Server,
    }

    public class WDBField
    {
        public int Col { get; private set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        public string Type { get; set; } = "none";
        public string Platform { get; set; } = "cs";

        private string defaultValue;
        public string DefaultValue
        {
            get
            {
                string value = string.IsNullOrEmpty(defaultValue) ? GetInnerDefaultValue() : defaultValue;
                return string.IsNullOrEmpty(value) ? null : value;
            }
            set
            {
                defaultValue = value;
            }
        }
        public string ValidationRule { get; set; } = null;

        internal WDBFieldType FieldType
        {
            get
            {
                if (string.IsNullOrEmpty(Type) || !Enum.TryParse<WDBFieldType>(Type, true, out var ft))
                {
                    return WDBFieldType.None;
                }
                else
                {
                    return ft;
                }
            }
        }

        internal WDBFieldPlatform FieldPlatform
        {
            get
            {
                if (Platform == "c")
                {
                    return WDBFieldPlatform.Client;
                }
                else if (Platform == "s")
                {
                    return WDBFieldPlatform.Server;
                }
                else
                {
                    return WDBFieldPlatform.All;
                }
            }
        }

        internal WDBValueValidation[] ValueValidations
        {
            get
            {
                List<string> rules = new List<string>();
                if (!string.IsNullOrEmpty(ValidationRule))
                {
                    string[] splitRules = ValidationRule.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if (splitRules != null && splitRules.Length > 0)
                    {
                        rules.AddRange(splitRules);
                    }
                }
                string[] innerRules = GetInnerValidationRule();
                if (innerRules != null && innerRules.Length > 0)
                {
                    rules.AddRange(innerRules);
                }
                return WDBUtility.CreateValidation(rules.ToArray());
            }
        }

        public WDBField(int col)
        {
            Col = col;
        }

        protected virtual string GetInnerDefaultValue()
        {
            return null;
        }

        protected virtual string[] GetInnerValidationRule()
        {
            return null;
        }

        public override string ToString()
        {
            return $"{{col = {Col}, name = {Name},desc = {Desc},type = {Type},platform = {Platform},defaultvalue = {DefaultValue},validationRule = {ValidationRule}}}";
        }
    }
}
