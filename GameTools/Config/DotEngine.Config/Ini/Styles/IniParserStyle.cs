namespace DotEngine.Config.Ini
{
    public class IniParserStyle
    {
        public bool ThrowExceptionsOnError { get; set; } = true;

        public bool AllowKeysWithoutSection { get; set; } = true;
        public bool AllowDuplicateSections { get; set; } = false;
        public bool SkipInvalidLines { get; set; } = false;
        public bool TrimProperties { get; set; } = true;
        public bool IsTrimSections { get; set; } = true;
        public bool IsTrimComments { get; set; } = true;
        public bool IsParseComments { get; set; } = true;
    }
}
