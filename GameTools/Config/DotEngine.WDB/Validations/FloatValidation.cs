using System.Collections.Generic;

namespace DotEngine.WDB
{
    public class FloatValidation : WDBValidation
    {
        protected override bool DoVerfy(List<string> errors)
        {
            string cellValue = GetCellValue();
            if (!string.IsNullOrEmpty(cellValue))
            {
                if (!float.TryParse(cellValue, out var value))
                {
                    errors.Add(GetErrorMessage(WDBConst.VALIDATION_CELL_CONVERT_ERR, cellValue, "float"));
                    return false;
                }
            }
            return true;
        }
    }
}
