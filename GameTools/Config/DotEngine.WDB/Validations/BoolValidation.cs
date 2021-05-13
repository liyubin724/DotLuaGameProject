using System.Collections.Generic;

namespace DotEngine.WDB.Validations
{
    public class BoolValidation : WDBValidation
    {
        protected override bool DoVerfy(List<string> errors)
        {
            string cellValue = GetCellValue();
            if(!string.IsNullOrEmpty(cellValue))
            {
                if(!bool.TryParse(cellValue,out var value))
                {
                    errors.Add(GetErrorMessage(WDBConst.VALIDATION_CELL_CONVERT_ERR, cellValue, "bool"));
                    return false;
                }
            }
            return true;
        }
    }
}
