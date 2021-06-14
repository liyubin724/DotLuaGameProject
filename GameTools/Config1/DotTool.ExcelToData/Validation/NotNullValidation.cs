using DotEngine.Context;
using DotTool.ETD.Data;
using DotTool.ETD.Log;

namespace DotTool.ETD.Validation
{
    public class NotNullValidation : IValidation
    {
#pragma warning disable CS0649
        [ContextField(typeof(LogHandler))]
        private LogHandler logHandler;
        [ContextField(typeof(Field))]
        private Field field;
        [ContextField(typeof(Cell))]
        private Cell cell;
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
            if (string.IsNullOrEmpty(content))
            {
                logHandler.Log(LogType.Error, LogMessage.LOG_VALIDATION_NULL, cell.Row, cell.Col);
                return ValidationResult.ContentIsNull;
            }

            return ValidationResult.Success;
        }
    }
}
