using DotEngine.Injection;
using System.Collections.Generic;

namespace DotEngine.Config.WDB
{
    public static class WDBContextKey
    {
        public const string ERROR_NAME = "__errors__";
        public const string CURRENT_SHEET_NAME = "__sheet__";
        public const string CURRENT_ROW_NAME = "__row__";
        public const string CURRENT_FIELD_NAME = "__field__";
        public const string CURRENT_CELL_NAME = "__cell__";
    }

    public class WDBContext : InjectStringContext
    {
        private List<string> errors = new List<string>();

        public WDBContext()
        {
            Add(WDBContextKey.ERROR_NAME, errors);
        }

        public bool HasError() => errors.Count > 0;
        public void AppendError(string message) => errors.Add(message);
        public string[] GetErrors() => errors.ToArray();
    }
}
