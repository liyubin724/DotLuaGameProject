using DotEngine.Context.Attributes;
using DotEngine.Context.Interfaces;
using System.Collections.Generic;

namespace DotEngine.Config.WDB
{
    public abstract class WDBValueValidation : IContextObject
    {
        public string Rule { get; private set; }
        public string[] Values { get; private set; }

        [ContextIE(WDBVerifyConst.CONTEXT_ERRORS_NAME)]
        protected List<string> errors;
        [ContextIE(WDBVerifyConst.CONTEXT_SHEET_NAME)]
        protected WDBSheet sheet;
        [ContextIE(WDBVerifyConst.CONTEXT_FIELD_NAME)]
        protected WDBField field;
        [ContextIE(WDBVerifyConst.CONTEXT_CELL_NAME)]
        protected WDBCell cell;

        public virtual void SetRule(string rule,params string[] values)
        {
            Rule = rule;
            Values = values;
        }

        public bool Verify()
        {
            if(field == null || cell == null)
            {
                errors.Add(GetErrorMsg(WDBVerifyConst.VALIDATION_CELL_ARG_NULL_ERR));
                return false;
            }
            return DoVerify();
        }

        protected abstract bool DoVerify();

        protected string GetErrorMsg(string format,params object[] values)
        {
            return WDBVerifyConst.GetCellErrorMsg(cell.Row, cell.Col, format, values);
        }
    }
}
