namespace DotEngine.Config.WDB
{
    public class LongValidation : WDBValueValidation
    {
        protected override bool DoVerify()
        {
            string cellValue = cell.GetValue(field);
            if (!string.IsNullOrEmpty(cellValue))
            {
                if (!long.TryParse(cellValue, out var value))
                {
                    errors.Add(GetErrorMsg(WDBVerifyConst.VALIDATION_CELL_CONVERT_ERR, cellValue, "long"));
                    return false;
                }
            }
            return true;
        }
    }


}
