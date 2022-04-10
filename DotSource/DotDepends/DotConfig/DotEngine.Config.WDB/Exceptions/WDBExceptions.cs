using System;

namespace DotEngine.Config.WDB
{
    public class WDBFieldTypeEmptyException : Exception
    {
        public WDBFieldTypeEmptyException(int column, string name)
            : base($"The type of the field({name}) at {column} is empty")
        {

        }
    }

    public class WDBValidationParserException : Exception
    {
        public WDBValidationParserException(int column, string rule)
            : base($"The rule({rule}) of the field({column}) can't be parsed.")
        {
        }
    }

    public class WDBValidationNotFoundException : Exception
    {
        public WDBValidationNotFoundException(int column, string rule)
            : base($"the validation({rule}) of field({column}) is not found.")
        {

        }
    }
}
