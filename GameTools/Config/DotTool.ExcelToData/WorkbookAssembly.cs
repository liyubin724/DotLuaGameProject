//using DotEngine.Context;
//using DotTool.ETD.Data;
//using DotTool.ETD.Log;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DotTool.ETD
//{
//    public class WorkbookAssembly
//    {
//        private TypeContext context;
//        private LogHandler logHandler;
//        private Dictionary<string, Workbook> workbookDic = new Dictionary<string, Workbook>();

//        public WorkbookAssembly(OnHandlerLog logHandler)
//        {
//            this.logHandler = new LogHandler(logHandler);

//            context = new TypeContext();
//            context.Add(typeof(LogHandler), this.logHandler);
//        }

//        public Workbook LoadFromFile(string excelPath)
//        {
//            if (string.IsNullOrEmpty(excelPath))
//            {
//                logHandler.Log(LogType.Error, LogMessage.LOG_ARG_IS_NULL);
//                return null;
//            }

//            if (!File.Exists(excelPath))
//            {
//                logHandler.Log(LogType.Error, LogMessage.LOG_FILE_NOT_EXIST, excelPath);
//                return null;
//            }

//        }

//        public Workbook[] LoadFromDir(string fileDir)
//        {

//        }
//    }
//}
