using DotEngine.Context;
using DotTool.ScriptGenerate;
using System.Collections.Generic;
using System.IO;

namespace ScriptGenerateDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string templateContent =
@"using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
<%
string spaceName = context.Get<string>(""spaceName"");
string className = context.Get<string>(""className"");
List<int> list = context.Get<List<int>>(""values"");
%>

<%+using System.Collections.Generic;%>
namespace <%=spaceName%>
{
    public class <%=className%>
    {
        public void Print()
        {
            int[] values = {<%foreach(var value in list){%><%=value%>,<%}%>};
            foreach(var value in values)
            {
                Console.WriteLine(value);
            }
        }
    }
}
";
            StringContext context = new StringContext();
            context.Add("spaceName", "Com.Test");
            context.Add("className", "TestClass");
            context.Add("values", new List<int>() { 1, 2, 3, 4, 5, 6 });

            string output = TemplateEngine.Execute(context, templateContent, null);
            File.WriteAllText("D:/t.cs", output);
        }
    }
}
