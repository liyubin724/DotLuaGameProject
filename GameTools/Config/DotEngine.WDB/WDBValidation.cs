using DotEngine.Context.Attributes;
using System.Collections.Generic;

namespace DotEngine.WDB
{
    public abstract class WDBValidation
    {
        public string Rule { get; private set; }

        [ContextIE(WDBConst.CONTEXT_SHEET_NAME)]
        protected WDBSheet sheet;
        [ContextIE(WDBConst.CONTEXT_FIELD_NAME)]
        protected WDBField field;
        [ContextIE(WDBConst.CONTEXT_CELL_NAME)]
        protected WDBCell cell;

        private List<string> tempErrors = new List<string>();

        public WDBValidation(string rule)
        {
            Rule = rule;
        }

        public string GetCellValue()
        {
            string value = cell.Value;
            if (string.IsNullOrEmpty(value))
            {
                value = field.DefaultValue;
            }
            return value;
        }

        public string GetErrorMessage(string msgFormat, params object[] values)
        {
            return WDBConst.GetCellErrorMsg(cell.Row, cell.Col, msgFormat, values);
        }

        public bool Verify(out string[] errors)
        {
            if(field == null || cell == null)
            {
                errors = new string[] { GetErrorMessage(WDBConst.VALIDATION_CELL_ARG_NULL_ERR) };
                return false;
            }

            if(DoVerfy(tempErrors))
            {
                errors = null;
                return true;
            }else
            {
                errors = tempErrors.ToArray();
                tempErrors.Clear();
                return false;
            }
        }

        protected abstract bool DoVerfy(List<string> errors);
    }
}
