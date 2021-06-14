namespace DotEngine.WDB
{
    public class LongValidation : WDBCellValidation
    {
        protected override void DoVerify()
        {
            string cellValue = cell.GetValue(field);
            if (!string.IsNullOrEmpty(cellValue))
            {
                if (!long.TryParse(cellValue, out var value))
                {
                    errors.Add(GetErrorMsg(WDBConst.VALIDATION_CELL_CONVERT_ERR, cellValue, "long"));
                }
            }
        }
    }


}
