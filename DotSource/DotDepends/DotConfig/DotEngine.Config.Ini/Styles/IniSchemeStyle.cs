namespace DotEngine.Config.Ini
{
    public class IniSchemeStyle
    {
        private string commentStr = "#";
        private string sectionStartStr = "[";
        private string sectionEndStr = "]";
        private string optionalValueStartStr = "--{";
        private string optionalValueEndStr = "}";
        private string optionalValueAssigmentStr = ",";
        private string propertyAssigmentStr = "=";

        public string CommentString
        {
            get
            {
                if(string.IsNullOrEmpty(commentStr))
                {
                    commentStr = "#";
                }
                return commentStr;
            }
            set
            {
                if(!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if(!string.IsNullOrEmpty(value))
                    {
                        commentStr = value;
                    }
                }
            }
        }

        public string SectionStartString
        {
            get
            {
                if (string.IsNullOrEmpty(sectionStartStr))
                {
                    sectionStartStr = "[";
                }
                return sectionStartStr;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        sectionStartStr = value;
                    }
                }
            }
        }

        public string SectionEndString
        {
            get
            {
                if (string.IsNullOrEmpty(sectionEndStr))
                {
                    sectionEndStr = "]";
                }
                return sectionEndStr;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        sectionEndStr = value;
                    }
                }
            }
        }

        public string OptionalValueStartString
        {
            get
            {
                if (string.IsNullOrEmpty(optionalValueStartStr))
                {
                    optionalValueStartStr = "{";
                }
                return optionalValueStartStr;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        optionalValueStartStr = value;
                    }
                }
            }
        }

        public string OptionalValueEndString
        {
            get
            {
                if (string.IsNullOrEmpty(optionalValueEndStr))
                {
                    optionalValueEndStr = "}";
                }
                return optionalValueEndStr;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        optionalValueEndStr = value;
                    }
                }
            }
        }

        public string OptionalValueAssigmentString
        {
            get
            {
                if (string.IsNullOrEmpty(optionalValueAssigmentStr))
                {
                    optionalValueAssigmentStr = ";";
                }
                return optionalValueAssigmentStr;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        optionalValueAssigmentStr = value;
                    }
                }
            }
        }

        public string PropertyAssigmentString
        {
            get
            {
                if (string.IsNullOrEmpty(propertyAssigmentStr))
                {
                    propertyAssigmentStr = "=";
                }
                return propertyAssigmentStr;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        propertyAssigmentStr = value;
                    }
                }
            }
        }
    }
}
