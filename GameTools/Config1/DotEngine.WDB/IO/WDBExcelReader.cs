using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.WDB.IO
{
    public class WDBExcelReader
    {
        public static WDBSheet[] ReadSheetFromFile(string excelDir)
        {

        }


        private static bool IsExcelFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) && !File.Exists(filePath))
            {
                return false;
            }

            string ext = Path.GetExtension(filePath);
            return !string.IsNullOrEmpty(ext) && (ext == ".xls" || ext == ".xlsx");
        }
    }
}
