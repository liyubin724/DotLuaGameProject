namespace DotEngine.Config.WDB
{
    [CustomValidation(WDBValidationNames.INT_NAME)]
    public class WDBIntCellValidation : WDBCellValidation
    {
        public override void Verify(WDBContext context)
        {
            WDBField field = GetField(context);
            WDBCell cell = GetCell(context);

            string cellContent = cell.GetContent(field);
            if (!int.TryParse(cellContent, out _))
            {
                AddErrorMessage(context, string.Format(WDBErrorMessages.CELL_PARSE_ERROR, cellContent, cell.Row, cell.Column, typeof(int)));
            }
        }
    }
}
