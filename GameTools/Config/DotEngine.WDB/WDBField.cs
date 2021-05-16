using DotEngine.Context;
using System;
using System.Collections.Generic;

namespace DotEngine.WDB
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

    public class WDBField : IVerify
    {
        public int Col { get; private set; }
        public string Name { get; set; }
        public string Desc { get; set; }

        private string type = "none";
        public string Type
        {
            get
            {
                return type;
            }
            set
            {
                if (string.IsNullOrEmpty(value) || !Enum.TryParse<WDBFieldType>(value, true, out var ft))
                {
                    type = "none";
                    fieldType = WDBFieldType.None;
                }
                else
                {
                    type = value;
                    fieldType = ft;
                }
            }
        }

        private string platform = "cs";
        public string Platform
        {
            get
            {
                return platform;
            }
            set
            {
                platform = value;
                if (platform == "cs")
                {
                    fieldPlatform = WDBFieldPlatform.All;
                }
                else if (platform == "c")
                {
                    fieldPlatform = WDBFieldPlatform.Client;
                }
                else if (platform == "s")
                {
                    fieldPlatform = WDBFieldPlatform.Server;
                }
                else
                {
                    platform = "cs";
                    fieldPlatform = WDBFieldPlatform.All;
                }
            }
        }

        private string defaultValue = string.Empty;
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

        private string validationRule = string.Empty;
        public string ValidationRule
        {
            get
            {
                return validationRule;
            }
            set
            {
                validations = null;
                validationRule = value;

                List<string> rules = new List<string>();
                if(!string.IsNullOrEmpty(validationRule))
                {
                    string[] splitRules = validationRule.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    if(splitRules != null && splitRules.Length>0)
                    {
                        rules.AddRange(splitRules);
                    }
                }
                string[] innerRules = GetInnerValidationRule();
                if(innerRules!=null && innerRules.Length>0)
                {
                    rules.AddRange(innerRules);
                }
                validations = WDBUtility.CreateValidation(rules.ToArray());
            }
        }

        private WDBValidation[] validations = null;
        public WDBValidation[] Validations
        {
            get
            {
                return validations;
            }
        }

        private WDBFieldType fieldType = WDBFieldType.None;
        public WDBFieldType FieldType
        {
            get
            {
                return fieldType;
            }
            set
            {
                fieldType = value;
                Type = fieldType.ToString().ToLower();
            }
        }

        private WDBFieldPlatform fieldPlatform = WDBFieldPlatform.All;
        public WDBFieldPlatform FieldPlatform
        {
            get
            {
                return fieldPlatform;
            }
            set
            {
                fieldPlatform = value;
                if (fieldPlatform == WDBFieldPlatform.Client)
                {
                    Platform = "c";
                }
                else if (fieldPlatform == WDBFieldPlatform.Server)
                {
                    Platform = "s";
                }
                else if (fieldPlatform == WDBFieldPlatform.All)
                {
                    Platform = "cs";
                }
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

        public void Verify(StringContextContainer context)
        {
            List<string> errors = context.Get<List<string>>(WDBConst.CONTEXT_ERRORS_NAME);
            if (string.IsNullOrEmpty(Name))
            {
                errors.Add(string.Format(WDBConst.VERIFY_FIELD_NAME_EMPTY_ERR, Col));
            }
            if (FieldType == WDBFieldType.None)
            {
                errors.Add(string.Format(WDBConst.VERIFY_FIELD_TYPE_NONE_ERR, Col,Name));
            }
            if (validations != null && validations.Length > 0)
            {
                foreach (var validation in validations)
                {
                   if(validation.GetType() == typeof(ErrorValidation))
                    {
                        errors.Add(string.Format(WDBConst.VERIFY_FIELD_VALIDATIONS_ERR, Col, Name, validationRule));
                    }
                }
            }
        }

        public override string ToString()
        {
            return $"{{col = {Col}, name = {Name},desc = {Desc},type = {Type},platform = {Platform},defaultvalue = {DefaultValue},validationRule = {ValidationRule}}}";
        }
    }
}
