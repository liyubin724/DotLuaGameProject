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
            string filePath = @"E:\GitHub\C#\C#-Ini\ini-parser\src\IniParser.Example\TestIniFile.ini";
            string fileContent = File.ReadAllText(filePath);
            if(IniReader.ReadFromString(fileContent,out var iniData))
            {
                Console.WriteLine("Success");
            }

            Console.ReadKey();
        }
    }
}
