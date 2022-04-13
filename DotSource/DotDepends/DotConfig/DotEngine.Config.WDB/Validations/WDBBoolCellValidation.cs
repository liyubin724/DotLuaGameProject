namespace DotEngine.Config.WDB
{
    [CustomValidation(WDBCellValidationNames.BOOL_NAME)]
    public class WDBBoolCellValidation : WDBCellValidation
    {
        public override void Verify(WDBContext context)
        {
            WDBField field = GetField(context);
            WDBCell cell = GetCell(context);

            string cellContent = cell.GetContent(field);
            if(!bool.TryParse(cellContent,out _))
            {
                context.AppendError(string.Format(WDBErrorMessages.CELL_CONTENT_CONVERT_ERROR, cellContent, cell.Row, cell.Column, typeof(bool)));
            }
        }
    }
}
