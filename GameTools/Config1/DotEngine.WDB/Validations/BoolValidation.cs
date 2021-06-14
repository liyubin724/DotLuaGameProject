﻿namespace DotEngine.WDB.Validations
{
    public class BoolValidation : WDBCellValidation
    {
        protected override void DoVerify()
        {
            string cellValue = cell.GetValue(field);
            if (!string.IsNullOrEmpty(cellValue))
            {
                if (!bool.TryParse(cellValue, out var value))
                {
                    errors.Add(GetErrorMsg(WDBConst.VALIDATION_CELL_CONVERT_ERR, cellValue, "bool"));
                }
            }
        }
    }
}
