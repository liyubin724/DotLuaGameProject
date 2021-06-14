using DotEngine.Context;
using DotTool.ETD.Data;
using DotTool.ETD.Log;

namespace DotTool.ETD.Validation
{
    public class UniqueValidation : IValidation
    {
#pragma warning disable CS0649
        [ContextField(typeof(LogHandler))]
        private LogHandler logHandler;
        [ContextField(typeof(Field))]
        private Field field;
        [ContextField(typeof(Cell))]
        private Cell cell;
        [ContextField(typeof(Sheet))]
        private Sheet sheet;
#pragma warning restore CS0649

        public string Rule { get; set; }

        public ValidationResult Verify()
        {
            if (field == null || cell == null)
            {
                logHandler.Log(LogType.Error, LogMessage.LOG_ARG_IS_NULL);
                return ValidationResult.ArgIsNull;
            }

            string content = cell.GetContent(field);
            for (int i = 0; i < sheet.LineCount; ++i)
            {
                Line line = sheet.GetLineByIndex(i);
                if (line.Row != cell.Row)
                {
                    Cell tempCell = line.GetCellByCol(field.Col);
                    string tempContent = tempCell.GetContent(field);
                    if (tempContent == content)
                    {
                        logHandler.Log(LogType.Error, LogMessage.LOG_VALIDATION_CONTENT_REPEAT_ERROR, cell.ToString(), tempCell.ToString());
                        return ValidationResult.ContentRepeatError;
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
