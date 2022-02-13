using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XLua;

namespace DotTools.CodeGen
{
    public static class Generator
    {
        public static string Generate(string templateContent, Dictionary<string, object> envDic)
        {
            LuaEnv luaEnv = new LuaEnv();

            luaEnv.DoString(Properties.Resources.TemplateCommon);

            LuaTable envTable = LuaTable.CreateFromDictionary(luaEnv, envDic);
            luaEnv.Global.Set("context", envTable);
            string genCode = Compiler.Run(luaEnv, templateContent);
            envTable.Dispose();

            luaEnv.Dispose();

            return genCode;
        }

        static void Main(string[] args)
        {
            Dictionary<string, object> envDic = new Dictionary<string, object>();
            envDic.Add("className", "TestClass");

            string templateContent = Properties.Resources.TestTemplate;
            string result = Generate(templateContent, envDic);

            Console.WriteLine(result);

            Console.ReadKey();
        }
    }
}
