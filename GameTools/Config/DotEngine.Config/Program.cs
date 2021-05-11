using DotEngine.Config.Ini;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotEngine.Config
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "D:/org.ini";
            string fileContent = File.ReadAllText(filePath);
            var iniData = IniReader.ReadFromString(fileContent);

            if(iniData!=null)
            {
                string content = IniWriter.WriteToString(iniData);
                File.WriteAllText("D:/test.ini", content);
            }

            Console.ReadKey();
        }
    }
}
