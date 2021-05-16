using System.Collections.Generic;

namespace DotEngine.WDB.Validations
{
    public class StrMinMaxLenValidation : WDBValidation
    {
        private int minLength = -1;
        private int maxLength = -1;

        public override void SetRule(string rule, params string[] values)
        {
            base.SetRule(rule, values);

            if(!int.TryParse(values[0],out minLength))
            {
                minLength = -1;
            }
            if(!int.TryParse(values[1],out maxLength))
            {
                maxLength = -1;
            }
        }

        protected override bool DoVerfy(List<string> errors)
        {
            if(minLength <0 || maxLength<0 || maxLength<minLength)
            {
                errors.Add(GetErrorMessage(WDBConst.VALIDATION_FIELD_RULE_PARAM_ERR,Rule));
                return false;
            }

            string cellValue = GetCellValue();
            int len = string.IsNullOrEmpty(cellValue) ? 0 : cellValue.Length;
            if(len<minLength)
            {
                errors.Add(GetErrorMessage(WDBConst.VALIDATION_CELL_STRING_TOO_SHORT_ERR));
                return false;
            }

            if(len > maxLength)
            {
                errors.Add(GetErrorMessage(WDBConst.VALIDATION_CELL_STRLen_TOO_LONG_ERR));
                return false;
            }
            return true;
        }
    }
}
