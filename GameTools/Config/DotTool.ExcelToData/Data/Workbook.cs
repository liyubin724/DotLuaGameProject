using DotEngine.Context;
using DotTool.ETD.Log;
using DotTool.ETD.Verify;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DotTool.ETD.Data
{
    public class Workbook : IVerify
    {
        private string filePath;
        public string FilePath { get => filePath; }
        public string Name { get => Path.GetFileNameWithoutExtension(filePath); }

        private List<Sheet> sheets = new List<Sheet>();

        public int SheetCount { get => sheets.Count; }
        public string[] SheetNames
        {
            get
            {
                return (from sheet in sheets select sheet.Name).ToArray();
            }
        }

        public Workbook(string path)
        {
            filePath = path;
        }
        
        public Sheet GetSheetByName(string name)
        {
            foreach(var sheet in sheets)
            {
                if(sheet.Name == name)
                {
                    return sheet;
                }
            }
            return null;
        }

        public Sheet GetSheeetByIndex(int index)
        {
            if(index>=0 && index<sheets.Count)
            {
                return sheets[index];
            }
            return null;
        }

        public void AddSheet(Sheet sheet)
        {
            sheets.Add(sheet);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(filePath);
            sb.AppendLine("===========================");
            foreach(var sheet in sheets)
            {
                sb.AppendLine(sheet.ToString());
            }
            sb.AppendLine();
            sb.AppendLine();
            return sb.ToString();
        }

        public bool Verify(TypeContext context)
        {
            LogHandler logHandler = context.Get<LogHandler>(typeof(LogHandler));

            context.Add(typeof(Workbook), this);

            bool result = true;
            foreach(var sheet in sheets)
            {
                if(!sheet.Verify(context))
                {
                    result = false;
                }
            }

            context.Remove(typeof(Workbook));
            return result;
        }
    }
}
