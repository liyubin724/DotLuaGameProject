using DotEngine.Context;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotTool.ScriptGenerate
{
    public class TemplateEngine
    {
        private static string[] DefaultAssemblies = new string[]
        {
            typeof(StringContext).Assembly.Location,
            typeof(StringBuilder).Assembly.Location,
        };

        private static string ScriptStart =
@"using DotEngine.Context;
using System.Text;

public static class TemplateRunner {
    public static string Run(StringContext context){
        StringBuilder output = new StringBuilder();";

        private static string ScriptEnd =
@"            return output.ToString();
    }
}";
        public static string Execute(StringContext context, string template, string[] assemblies)
        {
            string code = ComposeCode(template);

            //System.IO.File.WriteAllText("D:/code.cs", code);

            if (string.IsNullOrEmpty(code))
            {
                return null;
            }
            List<string> assemblyList = new List<string>(DefaultAssemblies);
            if (assemblies != null && assemblies.Length > 0)
            {
                assemblyList.AddRange(assemblies);
            }

            Assembly assembly = CompileCode(assemblyList.Distinct().ToArray(), code);
            if (assembly == null)
            {
                return null;
            }
            Type type = assembly.GetType("TemplateRunner");
            MethodInfo mInfo = type.GetMethod("Run", BindingFlags.Static | BindingFlags.Public);
            object result = mInfo.Invoke(null, new object[] { context });
            return result?.ToString();
        }

        private static Assembly CompileCode(string[] assemblies,string code)
        {
            CodeDomProvider provider = new CSharpCodeProvider();

            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.AddRange(assemblies);

            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = true;
            CompilerResults cr = provider.CompileAssemblyFromSource(parameters, code);
            if (cr.Errors.HasErrors)
            {
                foreach (var error in cr.Errors)
                {
                    Console.WriteLine(error.ToString());
                }
                return null;
            }
            else
            {
                return cr.CompiledAssembly;
            }
        }


        public static string ComposeCode(string templateContent)
        {
            StringBuilder code = new StringBuilder();

            code.AppendLine(ScriptStart);

            List<Chunk> chunks = ParseTemplate(templateContent);

            foreach(var chunk in chunks)
            {
                switch(chunk.Type)
                {
                    case TokenType.Text:
                        code.AppendLine($"output.Append(\"{ chunk.Text}\");");
                        break;
                    case TokenType.Code:
                        code.AppendLine(chunk.Text);
                        break;
                    case TokenType.Eval:
                        code.AppendLine($"output.Append(({ chunk.Text}).ToString());");
                        break;
                    case TokenType.Using:
                        code.Insert(0, chunk.Text+"\r\n");
                        break;
                    case TokenType.Ignore:
                        code.AppendLine($"/*{chunk.Text}*/");
                        break;
                }
            }

            code.AppendLine(ScriptEnd);

            return code.ToString();
        }

        private static List<Chunk> ParseTemplate(string templateContent)
        {
            if (string.IsNullOrEmpty(templateContent))
            {
                throw new TemplateFormatException("");
            }
            Regex regex = new Regex(GetRegexPattern(), RegexOptions.ExplicitCapture | RegexOptions.Singleline);
            Match matches = regex.Match(templateContent);
            if(!matches.Success)
            {
                throw new TemplateFormatException("");
            }
            if(matches.Groups["error"].Length>0)
            {
                throw new TemplateFormatException("");
            }

            List<Chunk> chunks = matches.Groups["text"].Captures
                .Cast<Capture>()
                .Select(p => new { Type = TokenType.Text, Value = EscapeSpecialCharacterToLiteral(p.Value), Index = p.Index })
                .Concat(matches.Groups["ignore"].Captures
                .Cast<Capture>()
                .Select(p => new { Type = TokenType.Ignore, Value = p.Value, Index = p.Index }))
                .Concat(matches.Groups["using"].Captures
                .Cast<Capture>()
                .Select(p => new { Type = TokenType.Using, Value = p.Value, Index = p.Index }))
                .Concat(matches.Groups["eval"].Captures
                .Cast<Capture>()
                .Select(p => new { Type = TokenType.Eval, Value = p.Value, Index = p.Index }))
                .Concat(matches.Groups["code"].Captures
                .Cast<Capture>()
                .Select(p => new { Type = TokenType.Code, Value = p.Value, Index = p.Index }))
                .OrderBy(p => p.Index)
                .Select(m => new Chunk(m.Type, m.Value))
                .ToList();

            if(chunks.Count == 0)
            {
                throw new TemplateFormatException("");
            }

            return chunks;
        }

        private static string GetRegexPattern()
        {
            string unopenedPattern = @"(?<error>((?!<%).)*%>)";
            string textPattern = @"(?<text>((?!<%).)+)";
            string ignorePattern = @"<%-(?<ignore>((?!<%|%>).)*)%>";
            string usingPattern = @"<%\+(?<using>((?!<%|%>).)*)%>";
            string evalPattern = @"<%=(?<eval>((?!<%|%>).)*)%>";
            string codePattern = @"<%(?<code>[^=|+|-]((?!<%|%>).)*)%>";
            string unclosedPattern = @"(?<error><%.*)";
            string emptyPattern = @"(?<error>^$)";

            string pattern = "(" +
                unopenedPattern +
                "|" + textPattern +
                "|" + usingPattern +
                "|" + codePattern +
                "|" + evalPattern +
                "|" + ignorePattern +
                "|" + unclosedPattern +
                "|" + emptyPattern +
                ")*";

            return pattern;
        }

        private static string EscapeSpecialCharacterToLiteral(string input)
        {
            return input.Replace("\\", @"\\")
                    .Replace("\'", @"\'")
                    .Replace("\"", @"\""")
                    .Replace("\n", @"\n")
                    .Replace("\t", @"\t")
                    .Replace("\r", @"\r")
                    .Replace("\b", @"\b")
                    .Replace("\f", @"\f")
                    .Replace("\a", @"\a")
                    .Replace("\v", @"\v")
                    .Replace("\0", @"\0");
        }
    }
}
