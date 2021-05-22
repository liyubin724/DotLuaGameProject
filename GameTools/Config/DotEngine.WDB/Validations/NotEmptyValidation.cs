namespace DotEngine.WDB
{
    public class NotEmptyValidation : WDBCellValidation
    {
        protected override void DoVerify()
        {
            string cellValue = cell.GetValue(field);
            if (string.IsNullOrEmpty(cellValue))
            {
                errors.Add(GetErrorMsg(WDBConst.VALIDATION_CELL_VAULE_EMPTY_ERR));
            }
        }
    }
}
