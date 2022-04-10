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
                context.AppendError(string.Format(WDBErrorMessages.CELL_CONTENT_CONVERT_ERROR, cellContent, cell.Row, cell.Column, typeof(long)));
            }
        }
    }
}
