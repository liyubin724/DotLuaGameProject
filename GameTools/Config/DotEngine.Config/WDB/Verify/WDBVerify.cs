using DotEngine.Context;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DotEngine.Config.WDB
{
    public class WDBFieldVerify
    {
        public WDBFieldType FieldType { get; set; } = WDBFieldType.None;
        public WDBFieldPlatform FieldPlatform { get; set; } = WDBFieldPlatform.All;
        public WDBValueValidation[] ValueValidations { get; set; } = null;
    }

    public static class WDBVerify
    {
        private static StringContextContainer context = new StringContextContainer();
        private static Dictionary<WDBField, WDBFieldVerify> fieldVerifyDic = new Dictionary<WDBField, WDBFieldVerify>();
        public static bool VerifySheets(WDBSheet[] sheets,out string[] errors)
        {
            errors = null;

            List<string> outputErrors = new List<string>();
            context.Add(WDBVerifyConst.CONTEXT_ERRORS_NAME, outputErrors);

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
            fieldVerifyDic.Clear();

            return outputErrors == null;
        }

        private static bool VerifySheet(WDBSheet sheet)
        {
            List<string> errors = (List<string>)context.Get(WDBVerifyConst.CONTEXT_ERRORS_NAME);
            if (string.IsNullOrEmpty(sheet.Name))
            {
                errors.Add(WDBVerifyConst.VERIFY_SHEET_NAME_EMPTY_ERR);
                return false;
            }
            if (!Regex.IsMatch(sheet.Name, WDBVerifyConst.VERIFY_SHEET_NAME_REGEX))
            {
                errors.Add(string.Format(WDBVerifyConst.VERIFY_SHEET_NAME_REGEX_ERR, sheet.Name));
                return false;
            }

            if (sheet.FieldCount == 0)
            {
                errors.Add(string.Format(WDBVerifyConst.VERIFY_SHEET_NO_FIELD_ERR, sheet.Name));
                return false;
            }
            if (sheet.LineCount == 0)
            {
                errors.Add(string.Format(WDBVerifyConst.VERIFY_SHEET_NO_ROW_ERR, sheet.Name));
                return false;
            }
            bool result = true;
            for (int i =0;i<sheet.FieldCount;++i)
            {
                WDBField field = sheet.GetFieldAtIndex(i);
                WDBFieldVerify fieldVerify = new WDBFieldVerify()
                {
                    FieldType = field.FieldType,
                    FieldPlatform = field.FieldPlatform,
                    ValueValidations = field.ValueValidations,
                };
                fieldVerifyDic.Add(field, fieldVerify);
                if(!VerifyField(field))
                {
                    result = false;
                }
            }
            if(!result)
            {
                return false;
            }

            context.Add(WDBVerifyConst.CONTEXT_SHEET_NAME, sheet);
            for(int i =0;i<sheet.LineCount; ++i)
            {
                WDBLine line = sheet.GetLineAtIndex(i);
                if(line.CellCount != sheet.FieldCount)
                {
                    result = false;
                    errors.Add(string.Format(WDBVerifyConst.VERIFY_SHEET_FIELD_ROW_ERR, sheet.FieldCount, line.Row));
                }
            }
            if(!result)
            {
                return false;
            }

            for(int i =0;i<sheet.FieldCount;++i)
            {
                WDBField field = sheet.GetFieldAtIndex(i);
                WDBFieldVerify fieldVerify = fieldVerifyDic[field];
                context.Add(WDBVerifyConst.CONTEXT_FIELD_NAME, field);
                context.Add(WDBVerifyConst.CONTEXT_FIELD_VERIFY_NAME, fieldVerify);

                for(int j = 0;j<sheet.LineCount;++j)
                {
                    WDBLine line = sheet.GetLineAtIndex(j);
                    WDBCell cell = line.GetCellByIndex(i);
                    context.Add(WDBVerifyConst.CONTEXT_LINE_NAME, line);
                    if(cell.Col != field.Col)
                    {
                        result = false;
                        errors.Add(string.Format(WDBVerifyConst.VERIFY_CELL_COL_NOTSAME_ERR, cell.Row, cell.Col));
                    }else
                    {
                        context.Add(WDBVerifyConst.CONTEXT_CELL_NAME, cell);
                        foreach (var cellValidation in fieldVerify.ValueValidations)
                        {
                            context.InjectTo(cellValidation);
                            if(!cellValidation.Verify())
                            {
                                result = false;
                            }
                        }
                        context.Remove(WDBVerifyConst.CONTEXT_CELL_NAME);
                    }
                    context.Remove(WDBVerifyConst.CONTEXT_LINE_NAME);
                }
                context.Remove(WDBVerifyConst.CONTEXT_FIELD_NAME);
                context.Remove(WDBVerifyConst.CONTEXT_FIELD_VERIFY_NAME);
            }

            return result;
        }

        private static bool VerifyField(WDBField field)
        {
            WDBFieldVerify fieldVerify = fieldVerifyDic[field];

            List<string> errors = (List<string>)context.Get(WDBVerifyConst.CONTEXT_ERRORS_NAME);
            if (string.IsNullOrEmpty(field.Name))
            {
                errors.Add(string.Format(WDBVerifyConst.VERIFY_FIELD_NAME_EMPTY_ERR, field.Col));
                return false;
            }
            if (!Regex.IsMatch(field.Name, WDBVerifyConst.VERIFY_FIELD_NAME_REGEX))
            {
                errors.Add(string.Format(WDBVerifyConst.VERIFY_FIELD_NAME_REGEX_ERR, field.Col));
                return false;
            }
            if (fieldVerify.FieldType == WDBFieldType.None)
            {
                errors.Add(string.Format(WDBVerifyConst.VERIFY_FIELD_TYPE_NONE_ERR, field.Col, field.Name));
                return false;
            }
            bool result = true;
            if(fieldVerify.ValueValidations!=null && fieldVerify.ValueValidations.Length>0)
            {
                foreach (var validation in fieldVerify.ValueValidations)
                {
                    if (validation.GetType() == typeof(ErrorValidation))
                    {
                        errors.Add(string.Format(WDBVerifyConst.VERIFY_FIELD_VALIDATIONS_ERR, field.Col, field.Name, field.ValidationRule));
                        result = false;
                    }
                }
            }
            return result;
        }
    }
}
