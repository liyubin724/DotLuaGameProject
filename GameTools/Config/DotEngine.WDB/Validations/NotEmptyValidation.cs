using System.Collections.Generic;

namespace DotEngine.WDB
{
    public class NotEmptyValidation : WDBValidation
    {
        protected override bool DoVerfy(List<string> errors)
        {
            string cellValue = GetCellValue();
            if (string.IsNullOrEmpty(cellValue))
            {
                errors.Add(GetErrorMessage(WDBConst.VALIDATION_CELL_VAULE_EMPTY_ERR));
                return false;
            }
            return true;
        }
    }
}
