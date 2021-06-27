using System.IO;

namespace DotEngine.Lua
{
    public class FileScriptLoader : ScriptLoader
    {
        protected override string GetFilePath(string scriptPath)
        {
            return LuaUtility.GetScriptFilePathInProject(scriptPath);
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
