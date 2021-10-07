using System.IO;
using UnityEngine;

namespace DotEngine.Lua
{
    public static class LuaScriptLoader
    {
        public static byte[] LoadScriptFromProject(ref string scriptPath)
        {
            scriptPath = $"{Application.dataPath}/Scripts/LuaScripts/{scriptPath}.txt";
            if (File.Exists(scriptPath))
            {
                return File.ReadAllBytes(scriptPath);
            }
            return null;
        }

    }
}