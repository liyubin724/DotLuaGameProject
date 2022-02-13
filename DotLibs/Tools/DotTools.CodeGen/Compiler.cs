using System;
using System.Collections.Generic;
using System.Text;
using XLua;

namespace DotTools.CodeGen
{
    public class Compiler
    {
        private static string ComposeCode(List<Chunk> chunks)
        {
            StringBuilder code = new StringBuilder();

            code.Append("local __text_gen = {}\r\n");
            foreach (var chunk in chunks)
            {
                switch (chunk.Token)
                {
                    case TokenType.Text:
                        code.Append("table.insert(__text_gen, \"" + chunk.Text + "\")\r\n");
                        break;
                    case TokenType.Eval:
                        code.Append("table.insert(__text_gen, tostring(" + chunk.Text + "))\r\n");
                        break;
                    case TokenType.Code:
                        code.Append(chunk.Text + "\r\n");
                        break;
                }
            }

            code.Append("return table.concat(__text_gen)\r\n");

            return code.ToString();
        }

        public static LuaFunction Compile(LuaEnv env, string templateContent)
        {
            string luaScript = ComposeCode(Scanner.Parse(templateContent));
            //System.Console.WriteLine(luaScript);
            return env.LoadString(luaScript, "Template");
        }
         
        public static string Execute(LuaFunction compiledTemplate)
        {
            object[] results = compiledTemplate.Call(new object[0], new Type[1] { typeof(string) });

             return results[0] as string;
        }

        public static string Run(LuaEnv env,string templateContent)
        {
            string result = string.Empty;

            LuaFunction luaFunc = Compile(env, templateContent);
            if (luaFunc != null)
            {
                result = Execute(luaFunc);    
            }
            luaFunc.Dispose();

            return result;
        }
    }
}
