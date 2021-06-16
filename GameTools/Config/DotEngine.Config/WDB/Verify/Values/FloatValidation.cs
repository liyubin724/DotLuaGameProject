﻿namespace DotEngine.Config.WDB
{
    public class FloatValidation : WDBValueValidation
    {
        protected override void DoVerify()
        {
            string cellValue = cell.GetValue(field);
            if (!string.IsNullOrEmpty(cellValue))
            {
                if (!float.TryParse(cellValue, out var value))
                {
                    errors.Add(GetErrorMsg(WDBVerifyConst.VALIDATION_CELL_CONVERT_ERR, cellValue, "float"));
                }
            }
        }
    }
}
