using DotEngine.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotEngine.Config.WDB
{
    public class WDBFieldVerify
    {
        public WDBFieldType Type { get; set; } = WDBFieldType.None;
        public WDBFieldPlatform Platform { get; set; } = WDBFieldPlatform.All;
        public WDBValueValidation[] Validations { get; set; } = null;
    }

   public static class WDBVerify
    {
        private static StringContextContainer context = new StringContextContainer();
        public static bool VerifySheets(WDBSheet[] sheets,out string[] errors)
        {
            errors = null;

            List<string> outputErrors = new List<string>();
            context.Add(WDBConst.CONTEXT_ERRORS_NAME, outputErrors);

            foreach(var sheet in sheets)
            {
                context.Add(sheet.Name, sheet);
            }

            foreach (var sheet in sheets)
            {
                VerifySheet(sheet);
            }

            if(outputErrors.Count>0)
            {
                errors = outputErrors.ToArray();
            }

            context.Clear();

            return outputErrors == null;
        }

        private static void VerifySheet(WDBSheet sheet)
        {
            List<string> errors = (List<string>)context.Get(WDBConst.CONTEXT_ERRORS_NAME);
            if (string.IsNullOrEmpty(sheet.Name))
            {
                errors.Add(WDBConst.VERIFY_SHEET_NAME_EMPTY_ERR);
                return;
            }
            if (!Regex.IsMatch(sheet.Name, WDBConst.VERIFY_SHEET_NAME_REGEX))
            {
                errors.Add(string.Format(WDBConst.VERIFY_SHEET_NAME_REGEX_ERR, sheet.Name));
                return;
            }

            if (sheet.FieldCount == 0)
            {
                errors.Add(string.Format(WDBConst.VERIFY_SHEET_NO_FIELD_ERR, sheet.Name));
                return;
            }
            if (sheet.RowCount == 0)
            {
                errors.Add(string.Format(WDBConst.VERIFY_SHEET_NO_ROW_ERR, sheet.Name));
                return;
            }

            context.Add(WDBConst.CONTEXT_SHEET_NAME, sheet);
            for(int i =0;i<sheet.FieldCount;++i)
            {

            }

        }

        private static void VerifyField(WDBField field)
        {
            List<string> errors = (List<string>)context.Get(WDBConst.CONTEXT_ERRORS_NAME);
            if (string.IsNullOrEmpty(field.Name))
            {
                errors.Add(string.Format(WDBConst.VERIFY_FIELD_NAME_EMPTY_ERR, field.Col));
                return false;
            }
            if (!Regex.IsMatch(field.Name, WDBConst.VERIFY_FIELD_NAME_REGEX))
            {
                errors.Add(string.Format(WDBConst.VERIFY_FIELD_NAME_REGEX_ERR, field.Col));
                return false;
            }
            if (field.FieldType == WDBFieldType.None)
            {
                errors.Add(string.Format(WDBConst.VERIFY_FIELD_TYPE_NONE_ERR, field.Col, field.Name));
                return false;
            }
        }

        private static void VerifyLine(WDBLine line)
        {

        }

        private static void VerifyCell(WDBField field,WDBCell cell)
        {

        }
    }
}
