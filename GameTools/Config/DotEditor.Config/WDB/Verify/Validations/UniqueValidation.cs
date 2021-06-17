using DotEngine.Config.WDB;
using System.Collections.Generic;

namespace DotEditor.Config.WDB
{
    public class UniqueValidation : WDBValueValidation
    {
        private static string CurrentSheetName = null;
        private static Dictionary<string, Dictionary<string, int>> contentRepeatedDic = new Dictionary<string, Dictionary<string, int>>();
        protected override bool DoVerify()
        {
            if (sheet.Name != CurrentSheetName)
            {
                CurrentSheetName = null;
            }
            if (CurrentSheetName == null)
            {
                CurrentSheetName = sheet.Name;
                contentRepeatedDic.Clear();
            }
            Dictionary<string, int> repeatedDic = null;
            if (!contentRepeatedDic.TryGetValue(field.Name, out repeatedDic))
            {
                repeatedDic = new Dictionary<string, int>();
                contentRepeatedDic[field.Name] = repeatedDic;

                for (int i = 0; i < sheet.LineCount; ++i)
                {
                    WDBLine tLine = sheet.GetLineAtIndex(i);
                    WDBCell tCell = tLine.GetCellByCol(field.Col);
                    string tValue = tCell.GetValue(field) ?? string.Empty;
                    if (repeatedDic.ContainsKey(tValue))
                    {
                        repeatedDic[tValue]++;
                    }
                    else
                    {
                        repeatedDic[tValue] = 1;
                    }
                }
            }

            string cellValue = cell.GetValue(field) ?? string.Empty;
            if (repeatedDic != null && repeatedDic.TryGetValue(cellValue, out var count) && count > 1)
            {
                errors.Add(GetErrorMsg(WDBVerifyConst.VALIDATION_CELL_UNIQUE_REPEAT_ERR, cellValue));
                return false;
            }
            return true;
        }
    }
}
