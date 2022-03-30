using DotEngine.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
