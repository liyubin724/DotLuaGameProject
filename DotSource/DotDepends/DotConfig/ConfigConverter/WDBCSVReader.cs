using CsvHelper;
using DotEngine.Config.WDB;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotTool.Config
{
    public static class WDBCSVReader
    {
        private static Action<LogType, string> sm_LogHandler;
        private static WDBCSVStyle sm_Style;

        public static WDBSheet[] ReadFromFile(string filePath, Action<LogType, string> logHandler, WDBCSVStyle style = null)
        {
            sm_LogHandler = logHandler;
            sm_Style = style ?? new WDBCSVStyle();

            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                return null;
            }

            List<WDBSheet> sheetList = new List<WDBSheet>();
            using (var reader = new StreamReader(filePath))
            {
                using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                {

                }
            }
        }
    }
}
