using DotEngine.Config.Ini;
using DotEngine.Services;

namespace DotEngine.Config
{
    public class IniConfigService : Servicer
    {
        public const string NAME = "IniConfigService";

        private IniConfig config = null;
        public IniConfigService() :base(NAME)
        {
        }

        public void SetConfigText(string configText)
        {
            config = new IniConfig();
            config.ParseText(configText);
        }

        public string GetValue(string key, string defaultValue = null)
        {
            return config.GetValue(key, defaultValue);
        }

        public string GetValueInGroup(string group, string key, string defaultValue = null)
        {
            return config.GetValueInGroup(group, key, defaultValue);
        }

        public bool GetBool(string key, bool defaultValue = false)
        {
            return config.GetBool(key, defaultValue);
        }

        public bool GetBoolInGroup(string group, string key, bool defaultValue = false)
        {
            return config.GetBoolInGroup(group, key, defaultValue);
        }

        public int GetInt(string key, int defaultValue = 0)
        {
            return config.GetInt(key, defaultValue);
        }

        public int GetIntInGroup(string group, string key, int defaultValue = 0)
        {
            return config.GetIntInGroup(group, key, defaultValue);
        }

        public float GetFloat(string key, float defaultValue = 0.0f)
        {
            return config.GetFloat(key, defaultValue);
        }

        public float GetFloatInGroup(string group, string key, float defaultValue = 0.0f)
        {
            return config.GetFloatInGroup(group, key, defaultValue);
        }
    }
}
