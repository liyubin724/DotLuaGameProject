namespace DotTool.ETD.Validation
{
    public enum ValidationResult
    {
        Success = 0,
        Pass = 1,

        ValidationFormatError = -1,
        ArgIsNull = -2,
        ContentIsNull = -3,
        ParseContentFailed = -4,
        NumberRangeError = -5,
        MaxLenError = -6,
        ContentRepeatError = -7,
        LuaFunctionError = -8,
    }

    public interface IValidation
    {
        string Rule { get; set; }
        ValidationResult Verify();
    }
}
