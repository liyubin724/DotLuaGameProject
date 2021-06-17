using DotEngine.Config.WDB;

namespace DotEditor.Config.WDB
{
    public class NotEmptyValidation : WDBValueValidation
    {
        protected override bool DoVerify()
        {
            string cellValue = cell.GetValue(field);
            if (string.IsNullOrEmpty(cellValue))
            {
                errors.Add(GetErrorMsg(WDBVerifyConst.VALIDATION_CELL_VAULE_EMPTY_ERR));
                return false;
            }

            return true;
        }
    }
}
