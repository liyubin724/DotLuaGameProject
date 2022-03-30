namespace DotEngine.Config.WDB
{
    [CustomValidation(WDBValidationNames.LONG_NAME)]
    public class WDBLongCellValidation : WDBCellValidation
    {
        public override void Verify(WDBContext context)
        {
            WDBField field = GetField(context);
            WDBCell cell = GetCell(context);

            string cellContent = cell.GetContent(field);
            if (!long.TryParse(cellContent, out _))
            {
                AddErrorMessage(context, string.Format(WDBErrorMessages.CELL_PARSE_ERROR, cellContent, cell.Row, cell.Column, typeof(long)));
            }
        }
    }
}
