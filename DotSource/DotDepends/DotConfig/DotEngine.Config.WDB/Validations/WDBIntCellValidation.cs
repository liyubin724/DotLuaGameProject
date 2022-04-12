namespace DotEngine.Config.WDB
{
    [CustomValidation(WDBCellValidationNames.INT_NAME)]
    public class WDBIntCellValidation : WDBCellValidation
    {
        public override void Verify(WDBContext context)
        {
            WDBField field = GetField(context);
            WDBCell cell = GetCell(context);

            string cellContent = cell.GetContent(field);
            if (!int.TryParse(cellContent, out _))
            {
                context.AppendError(string.Format(WDBErrorMessages.CELL_CONTENT_CONVERT_ERROR, cellContent, cell.Row, cell.Column, typeof(int)));
            }
        }
    }
}
