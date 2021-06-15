namespace DotEngine.Config.WDB
{
    public class StrMinMaxLenValidation : WDBCellValidation
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

        protected override void DoVerify()
        {
            if(minLength <0 || maxLength<0 || maxLength<minLength)
            {
                errors.Add(GetErrorMsg(WDBConst.VALIDATION_FIELD_RULE_PARAM_ERR,Rule));
            }

            string cellValue = cell.GetValue(field);
            int len = string.IsNullOrEmpty(cellValue) ? 0 : cellValue.Length;
            if(len<minLength)
            {
                errors.Add(GetErrorMsg(WDBConst.VALIDATION_CELL_STRING_TOO_SHORT_ERR));
            }
            if(len > maxLength)
            {
                errors.Add(GetErrorMsg(WDBConst.VALIDATION_CELL_STRLen_TOO_LONG_ERR));
            }
        }
    }
}
