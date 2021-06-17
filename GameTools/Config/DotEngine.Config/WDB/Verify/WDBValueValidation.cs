using DotEngine.Context.Attributes;
using DotEngine.Context.Interfaces;
using System.Collections.Generic;

namespace DotEngine.Config.WDB
{
    public static class WDBContextIENames
    {
        public const string CONTEXT_ERRORS_NAME = "__errors__";
        public const string CONTEXT_SHEET_NAME = "__sheet__";
        public const string CONTEXT_FIELD_NAME = "__field__";
        public const string CONTEXT_FIELD_VERIFY_NAME = "__fieldVerify__";
        public const string CONTEXT_LINE_NAME = "__line__";
        public const string CONTEXT_CELL_NAME = "__cell__";
    }

    public abstract class WDBValueValidation : IContextObject
    {
        public string Rule { get; private set; }
        public string[] Values { get; private set; }

        [ContextIE(WDBContextIENames.CONTEXT_ERRORS_NAME)]
        protected List<string> errors;
        [ContextIE(WDBContextIENames.CONTEXT_SHEET_NAME)]
        protected WDBSheet sheet;
        [ContextIE(WDBContextIENames.CONTEXT_FIELD_NAME)]
        protected WDBField field;
        [ContextIE(WDBContextIENames.CONTEXT_CELL_NAME)]
        protected WDBCell cell;

        public virtual void SetRule(string rule, params string[] values)
        {
            Rule = rule;
            Values = values;
        }

        public bool Verify()
        {
            if (field == null || cell == null)
            {
                errors.Add(GetErrorMsg("The argument is null."));
                return false;
            }
            return DoVerify();
        }

        protected abstract bool DoVerify();

        protected string GetErrorMsg(string format, params object[] values)
        {
            return string.Format(format, values) + string.Format("(row = {0},col = {1})", cell.Row, cell.Col);
        }
    }
}
