﻿using DotEngine.Config.WDB;

namespace DotEditor.Config.WDB
{
    public class StrMinMaxLenValidation : WDBValueValidation
    {
        private int minLength = -1;
        private int maxLength = -1;

        public override void SetRule(string rule, params string[] values)
        {
            base.SetRule(rule, values);

            if (!int.TryParse(values[0], out minLength))
            {
                minLength = -1;
            }
            if (!int.TryParse(values[1], out maxLength))
            {
                maxLength = -1;
            }
        }

        protected override bool DoVerify()
        {
            if (minLength < 0 || maxLength < 0 || maxLength < minLength)
            {
                errors.Add(GetErrorMsg(WDBVerifyConst.VALIDATION_FIELD_RULE_PARAM_ERR, Rule));
                return false;
            }

            string cellValue = cell.GetValue(field);
            int len = string.IsNullOrEmpty(cellValue) ? 0 : cellValue.Length;
            if (len < minLength)
            {
                errors.Add(GetErrorMsg(WDBVerifyConst.VALIDATION_CELL_STRLEN_TOO_SHORT_ERR, cellValue));
                return false;
            }
            if (len > maxLength)
            {
                errors.Add(GetErrorMsg(WDBVerifyConst.VALIDATION_CELL_STRLEN_TOO_LONG_ERR, cellValue));
                return false;
            }
            return true;
        }
    }
}
