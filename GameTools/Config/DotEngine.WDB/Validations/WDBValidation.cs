using DotEngine.Context.Attributes;
using System.Collections.Generic;

namespace DotEngine.WDB
{
    public abstract class WDBValidation
    {
        public string Rule { get; private set; }
        public string[] Values { get; private set; }

        public virtual void SetRule(string rule,params string[] values)
        {
            Rule = rule;
            Values = values;
        }

        public string[] Verify(WDBSheet sheet, WDBField field, WDBCell cell)
        {
            if(field == null || cell == null)
            {
                return  new string[] { WDBConst.GetCellErrorMsg(cell.Row,cell.Col,WDBConst.VALIDATION_CELL_ARG_NULL_ERR) };
            }else
            {
                return DoVerify(sheet,field,cell);
            }
        }

        protected abstract string[] DoVerify(WDBSheet sheet, WDBField field, WDBCell cell);
    }
}
