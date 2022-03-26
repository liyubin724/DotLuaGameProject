﻿using DotEngine.Config.Ini;
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
    public class IniReaderTest
    {
        [TestMethod]
        public void ReadFromFile()
        {
            string content = File.ReadAllText(@"D:\ini-config.txt");
            
            IniConfig config = IniReader.ReadFromString(content);
            Assert.IsTrue(config != null, "The config is null");
        }
    }
}
