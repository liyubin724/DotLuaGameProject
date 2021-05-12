using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public int Col { get; set; }
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
                if (string.IsNullOrEmpty(value))
                {
                    type = "none";
                    fieldType = WDBFieldType.None;
                    return;
                }

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
                return string.IsNullOrEmpty(value) ? string.Empty : value;
            }
            set
            {
                defaultValue = value;
            }
        }

        public string ValidationRule { get; set; }

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

        protected virtual string GetInnerDefaultValue()
        {
            return null;
        }
    }
}
