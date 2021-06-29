using System.IO;
using UnityEngine;

namespace DotEngine.Lua
{
    public class FileScriptLoader : ScriptLoader
    {
        protected override string GetFilePath(string scriptPath)
        {
            return $"{Application.dataPath}/Scripts/LuaScripts/{scriptPath}.txt";
        }

        protected override byte[] ReadBytes(string filePath)
        {
            if(File.Exists(filePath))
            {
                return File.ReadAllBytes(filePath);
            }
            return null;
        }
    }
}
