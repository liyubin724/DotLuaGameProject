using DotEngine.Log;

namespace DotEngine.Lua
{
    public abstract class ScriptLoader
    {
        private string[] pathFormats = new string[0];

        public ScriptLoader(string[] formats)
        {
            pathFormats = formats;
        }

        public byte[] LoadScript(ref string filePath)
        {
            if(pathFormats!=null && pathFormats.Length>0)
            {
                foreach (var f in pathFormats)
                {
                    string fp = string.Format(f, filePath);
                    if(IsExist(fp))
                    {
                        filePath = fp;
                        return ReadBytes(fp);
                    }
                }
            }
            LogUtil.Warning(LuaConst.LOGGER_NAME, $"ScriptLoader::LoadScript->Script not found.filePath = {filePath}");

            return null;
        }

        protected abstract bool IsExist(string filePath);
        protected abstract byte[] ReadBytes(string filePath);
    }
}
