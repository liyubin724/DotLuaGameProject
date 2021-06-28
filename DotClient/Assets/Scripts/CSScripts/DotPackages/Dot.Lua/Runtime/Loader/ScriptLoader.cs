using DotEngine.Log;

namespace DotEngine.Lua
{
    public abstract class ScriptLoader
    {
        public byte[] LoadScript(ref string scriptPath)
        {
            string filePath = GetFilePath(scriptPath);
            byte[] scriptBytes = ReadBytes(filePath);
            if (scriptBytes == null || scriptBytes.Length == 0)
            {
                LogUtil.Error(LuaUtility.LOG_TAG, $"load luaScript failed.scriptPath = {scriptPath}");
                return null;
            }

            scriptPath = filePath;

            return scriptBytes;
        }

        protected abstract string GetFilePath(string scriptPath);

        protected abstract byte[] ReadBytes(string scriptPath);
    }
}