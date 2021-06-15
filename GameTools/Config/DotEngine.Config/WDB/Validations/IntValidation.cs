namespace DotEngine.WDB
{
    public class IntValidation : WDBCellValidation
    {
        protected override void DoVerify()
        {
            string cellValue = cell.GetValue(field);
            if (!string.IsNullOrEmpty(cellValue))
            {
                if (!int.TryParse(cellValue, out var value))
                {
                    errors.Add(GetErrorMsg(WDBConst.VALIDATION_CELL_CONVERT_ERR, cellValue, "int"));
                }
            }
        }
    }
}
