namespace DotEngine.Ini
{
    public class IniReaderStyle
    {
        public bool ThrowExceptionsOnError { get; set; } = true;
        public bool AllowKeysWithoutSection { get; set; } = true;
        public bool IsParseComments { get; set; } = true;
        public bool IsTrimComments { get; set; } = true;
        public bool IsTrimOptionalValues { get; set; } = true;
        public bool IsParseOptionalValues { get; set; } = true;
        public bool IsTrimProperties { get; set; } = true;
        public bool IsTrimSections { get; set; } = true;
    }
}
