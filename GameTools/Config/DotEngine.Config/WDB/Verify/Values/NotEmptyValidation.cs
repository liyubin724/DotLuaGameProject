namespace DotEngine.Config.WDB
{
    public class NotEmptyValidation : WDBValueValidation
    {
        protected override void DoVerify()
        {
            string cellValue = cell.GetValue(field);
            if (string.IsNullOrEmpty(cellValue))
            {
                errors.Add(GetErrorMsg(WDBVerifyConst.VALIDATION_CELL_VAULE_EMPTY_ERR));
            }
        }
    }
}
