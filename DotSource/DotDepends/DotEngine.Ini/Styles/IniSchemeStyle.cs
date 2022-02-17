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
            get => string.IsNullOrEmpty(commentStr) ? ";" : commentStr;
            set => commentStr = value?.Trim();
        }

        public string SectionStartString
        {
            get => string.IsNullOrEmpty(sectionStartStr) ? "[" : sectionStartStr;
            set => sectionStartStr = value?.Trim();
        }

        public string SectionEndString
        {
            get => string.IsNullOrEmpty(sectionEndStr) ? "]" : sectionEndStr;
            set => sectionEndStr = value?.Trim();
        }

        public string OptionalValueStartString
        {
            get => string.IsNullOrEmpty(optionalValueStartStr) ? "{" : optionalValueStartStr;
            set => optionalValueStartStr = value?.Trim();
        }

        public string OptionalValueEndString
        {
            get => string.IsNullOrEmpty(optionalValueEndStr) ? "}" : optionalValueEndStr;
            set => optionalValueEndStr = value?.Trim();
        }

        public string OptionalValueAssigmentString
        {
            get => string.IsNullOrEmpty(optionalValueAssigmentStr) ? "=" : optionalValueAssigmentStr;
            set => optionalValueAssigmentStr = value?.Trim();
        }

        public string PropertyAssigmentString
        {
            get => string.IsNullOrEmpty(propertyAssigmentStr) ? "=" : propertyAssigmentStr;
            set => propertyAssigmentStr = value?.Trim();
        }
    }
}
