using System.IO;

namespace DotEngine.Lua
{
    public class FileScriptLoader : ScriptLoader
    {
        public FileScriptLoader(string[] formats) : base(formats)
        {
        }

        protected override bool IsExist(string filePath)
        {
            return File.Exists(filePath);
        }

        protected override byte[] ReadBytes(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }
    }
}
