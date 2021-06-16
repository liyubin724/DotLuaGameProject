namespace DotEngine.Config.WDB
{
    public class BoolValidation : WDBValueValidation
    {
        protected override bool DoVerify()
        {
            string cellValue = cell.GetValue(field);
            if (!string.IsNullOrEmpty(cellValue))
            {
                if (!bool.TryParse(cellValue, out var value))
                {
                    errors.Add(GetErrorMsg(WDBVerifyConst.VALIDATION_CELL_CONVERT_ERR, cellValue, "bool"));
                    return false;
                }
            }

            return true;
        }
    }
}
