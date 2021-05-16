using System.Collections.Generic;

namespace DotEngine.WDB
{
    public class IntValidation : WDBValidation
    {
        protected override bool DoVerify(List<string> errors)
        {
            string cellValue = GetCellValue();
            if (!string.IsNullOrEmpty(cellValue))
            {
                if (!int.TryParse(cellValue, out var value))
                {
                    errors.Add(GetErrorMessage(WDBConst.VALIDATION_CELL_CONVERT_ERR, cellValue, "int"));
                    return false;
                }
            }
            return true;
        }
    }
}
