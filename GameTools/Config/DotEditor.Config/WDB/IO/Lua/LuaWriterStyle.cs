using DotEngine.Config.WDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEditor.Config.WDB
{
    public class LuaWriterStyle
    {
        public string TemplateContent { get; set; }
        public string[] AssemblyNames { get; set; } = new string[] { typeof(WDBSheet).Assembly.Location };
        public string OutputFileDir { get; set; }
    }
}
