namespace DotEngine.Config.Ini
{
    public class IniParserStyle
    {
        public bool AllowKeysWithoutSection { get; set; } = true;
        public bool ThrowExceptionsOnError { get; set; } = true;
        public bool AllowDuplicateSections { get; set; } = false;
        public bool SkipInvalidLines { get; set; } = false;
        public bool TrimProperties { get; set; } = true;
        public bool TrimSections { get; set; } = true;
        public bool TrimComments { get; set; } = true;
        public bool ParseComments { get; set; } = true;
    }
}
