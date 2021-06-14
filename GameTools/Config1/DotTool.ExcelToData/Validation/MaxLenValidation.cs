using DotEngine.Context;
using DotTool.ETD.Data;
using DotTool.ETD.Log;
using System.Text.RegularExpressions;

namespace DotTool.ETD.Validation
{
    public class MaxLenValidation : IValidation
    {
        private const string MAX_LEN_REGEX = @"MaxLen\((?<len>[0-9]+)\)";

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
            int maxLen = 0;
            Match match = new Regex(MAX_LEN_REGEX).Match(Rule);
            Group group = match.Groups["len"];
            if (group.Success)
            {
                if (!int.TryParse(group.Value, out maxLen))
                {
                    maxLen = 0;
                }
            }
            if (maxLen <= 0)
            {
                logHandler.Log(LogType.Error, LogMessage.LOG_VALIDATION_FORMAT_ERROR, cell.Col, Rule);
                return ValidationResult.ValidationFormatError;
            }

            string content = cell.GetContent(field);
            if (string.IsNullOrEmpty(content))
            {
                return ValidationResult.Success;
            }

            if (content.Length > maxLen)
            {
                logHandler.Log(LogType.Error, LogMessage.LOG_VALIDATION_LEN_ERROR, maxLen, cell.Row, cell.Col, content);
                return ValidationResult.MaxLenError;
            }
            return ValidationResult.Success;
        }
    }
}
