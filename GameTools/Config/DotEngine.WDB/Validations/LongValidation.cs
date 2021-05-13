using System.Collections.Generic;

namespace DotEngine.WDB
{
    public class LongValidation : WDBValidation
    {
        public LongValidation(string rule) : base(rule)
        {
        }

        protected override bool DoVerfy(List<string> errors)
        {
            string cellValue = GetCellValue();
            if (!string.IsNullOrEmpty(cellValue))
            {
                if (!long.TryParse(cellValue, out var value))
                {
                    errors.Add(GetErrorMessage(WDBConst.VALIDATION_CELL_CONVERT_ERR, cellValue, "long"));
                    return false;
                }
            }
            return true;
        }
    }
}
