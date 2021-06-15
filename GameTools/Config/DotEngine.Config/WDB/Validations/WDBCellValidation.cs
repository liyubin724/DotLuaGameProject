using DotEngine.Context.Attributes;
using DotEngine.Context.Interfaces;
using System.Collections.Generic;

namespace DotEngine.Config.WDB
{
    public abstract class WDBCellValidation : IContextObject
    {
        public string Rule { get; private set; }
        public string[] Values { get; private set; }

        [ContextIE(WDBConst.CONTEXT_ERRORS_NAME)]
        protected List<string> errors;
        [ContextIE(WDBConst.CONTEXT_SHEET_NAME)]
        protected WDBSheet sheet;
        [ContextIE(WDBConst.CONTEXT_FIELD_NAME)]
        protected WDBField field;
        [ContextIE(WDBConst.CONTEXT_CELL_NAME)]
        protected WDBCell cell;

        public virtual void SetRule(string rule,params string[] values)
        {
            Rule = rule;
            Values = values;
        }

        public void Verify()
        {
            if(field == null || cell == null)
            {
                errors.Add(GetErrorMsg(WDBConst.VALIDATION_CELL_ARG_NULL_ERR));
                return;
            }
            DoVerify();
        }

        protected abstract void DoVerify();

        protected string GetErrorMsg(string format,params object[] values)
        {
            return WDBConst.GetCellErrorMsg(cell.Row, cell.Col, format, values);
        }
    }
}
