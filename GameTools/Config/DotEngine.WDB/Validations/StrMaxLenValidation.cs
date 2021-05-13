using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.WDB.Validations
{
    public class StrMaxLenValidation : WDBValidation
    {
        public StrMaxLenValidation(string rule) : base(rule)
        {
        }

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
