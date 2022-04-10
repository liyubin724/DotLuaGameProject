namespace DotEngine.Config.Ini
{
    public class IniSchemeStyle
    {
        private string commentPrefix = "#";
        private string sectionPrefix = "[";
        private string sectionPostfix = "]";
        private string optionalValuePrefix = "--{";
        private string optionalValuePostfix = "}";
        private string optionalValueAssigment = ";";
        private string propertyAssigment = "=";

        public string CommentPrefix
        {
            get
            {
                if (string.IsNullOrEmpty(commentPrefix))
                {
                    commentPrefix = "#";
                }
                return commentPrefix;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        commentPrefix = value;
                    }
                    else
                    {
                        commentPrefix = "#";
                    }
                }
            }
        }

        public string SectionPrefix
        {
            get
            {
                if (string.IsNullOrEmpty(sectionPrefix))
                {
                    sectionPrefix = "[";
                }
                return sectionPrefix;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        sectionPrefix = value;
                    }
                    else
                    {
                        sectionPrefix = "[";
                    }
                }
            }
        }

        public string SectionPostfix
        {
            get
            {
                if (string.IsNullOrEmpty(sectionPostfix))
                {
                    sectionPostfix = "]";
                }
                return sectionPostfix;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        sectionPostfix = value;
                    }
                    else
                    {
                        sectionPostfix = "]";
                    }
                }
            }
        }

        public string OptionalValuePrefix
        {
            get
            {
                if (string.IsNullOrEmpty(optionalValuePrefix))
                {
                    optionalValuePrefix = "{";
                }
                return optionalValuePrefix;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        optionalValuePrefix = value;
                    }
                    else
                    {
                        optionalValuePrefix = "--{";
                    }
                }
            }
        }

        public string OptionalValuePostfix
        {
            get
            {
                if (string.IsNullOrEmpty(optionalValuePostfix))
                {
                    optionalValuePostfix = "}";
                }
                return optionalValuePostfix;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        optionalValuePostfix = value;
                    }
                    else
                    {
                        optionalValuePostfix = "}";
                    }
                }
            }
        }

        public string OptionalValueAssigment
        {
            get
            {
                if (string.IsNullOrEmpty(optionalValueAssigment))
                {
                    optionalValueAssigment = ";";
                }
                return optionalValueAssigment;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        optionalValueAssigment = value;
                    }
                    else
                    {
                        optionalValueAssigment = ";";
                    }
                }
            }
        }

        public string PropertyAssigment
        {
            get
            {
                if (string.IsNullOrEmpty(propertyAssigment))
                {
                    propertyAssigment = "=";
                }
                return propertyAssigment;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if (!string.IsNullOrEmpty(value))
                    {
                        propertyAssigment = value;
                    }
                    else
                    {
                        propertyAssigment = "=";
                    }
                }
            }
        }
    }
}
