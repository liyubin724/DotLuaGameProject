using System;

namespace DotEngine.Config.WDB
{
    [CustomValidation(WDBCellValidationNames.DATETIME_NAME)]
    public class WDBDateTimeCellValidation : WDBCellValidation
    {
        public override void Verify(WDBContext context)
        {
            WDBField field = GetField(context);
            WDBCell cell = GetCell(context);

            string cellContent = cell.GetContent(field);
            if (!DateTime.TryParse(cellContent, out _))
            {
                context.AppendError(string.Format(WDBErrorMessages.CELL_CONTENT_CONVERT_ERROR, cellContent, cell.Row, cell.Column, typeof(DateTime)));
            }
        }
    }
}
