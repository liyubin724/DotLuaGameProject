using DotEngine.Context;
using DotTool.ScriptGenerate;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScriptGenerateDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            string templateFilePath = @"E:\WorkSpace\DotGameProject\DotGameScripts\DotFramework\DotNet\Protobuf-Tool\Generator\script-template\csharp\client-message-parser.txt";
            string output = TemplateEngine.Execute(new StringContext(), File.ReadAllText(templateFilePath), null);
            File.WriteAllText("D:/t.cs", output);
        }
    }
}
