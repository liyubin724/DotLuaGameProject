using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.WDB.Validations
{
    public class StrMaxLenValidation : WDBValidation
    {
        private int maxLength = int.MaxValue;

        public override void SetRule(string rule, params string[] values)
        {
            base.SetRule(rule, values);

            maxLength = int.Parse(values[0]);
        }

        protected override bool DoVerfy(List<string> errors)
        {
            if(maxLength<=0)
            {
                errors.Add(GetErrorMessage(WDBConst.VALIDATION_FIELD_RULE_PARAM_ERR,Rule));
                return false;
            }

            string cellValue = GetCellValue();
            if (!string.IsNullOrEmpty(cellValue))
            {
                errors.Add(GetErrorMessage(WDBConst.VALIDATION_CELL_VAULE_EMPTY_ERR));
                return false;
            }
            return true;
        }
    }
}
