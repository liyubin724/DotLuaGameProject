using DotEngine.Config.Ini;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotTest.Config.Ini
{
    [TestClass]
    public class IniWriterTester
    {
        [TestMethod]
        public void WriteToFile()
        {
            IniConfig config = new IniConfig();
            IniSection section = config.AddSection("TestSection");
            section.Comments.AddRange(new string[]
            {
                "Test1","Comment2"
            });

            IniProperty property = section.AddProperty("name", "Test", new string[] { "Property1", "TestProperty" }, new string[] { "Test1", "Test2" });

            string content = IniWriter.WriteToString(config);
            File.WriteAllText(@"D:\ini-config.txt", content);
        }
    }
}
