using Newtonsoft.Json;

namespace DotEngine.Core.IO
{
    public class JSONFormatSetting
    {
        public static JSONFormatSetting Default = new JSONFormatSetting();

        public bool IsIndent { get; set; } = true;
        public bool IsUsedClassType { get; set; } = false;
        public bool IsUseDefaultValue { get; set; } = false;

        public JsonSerializerSettings ToSerializeSetting()
        {
            JsonSerializerSettings jsSetting = new JsonSerializerSettings();
            if (IsIndent)
            {
                jsSetting.Formatting = Formatting.Indented;
            }
            else
            {
                jsSetting.Formatting = Formatting.None;
            }
            if (IsUsedClassType)
            {
                jsSetting.TypeNameHandling = TypeNameHandling.Auto;
                jsSetting.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full;
            }
            if (IsUseDefaultValue)
            {
                jsSetting.DefaultValueHandling = DefaultValueHandling.Ignore;
            }
            else
            {
                jsSetting.DefaultValueHandling = DefaultValueHandling.Include;
            }
            return jsSetting;
        }

        public JsonSerializerSettings ToDeserializeSetting()
        {
            JsonSerializerSettings jsSetting = new JsonSerializerSettings();
            if (IsUsedClassType)
            {
                jsSetting.TypeNameHandling = TypeNameHandling.Auto;
                jsSetting.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full;
            }
            return jsSetting;
        }

        public static implicit operator JsonSerializerSettings(JSONFormatSetting setting)
        {
            JsonSerializerSettings jsSetting = new JsonSerializerSettings();
            if (setting.IsIndent)
            {
                jsSetting.Formatting = Formatting.Indented;
            }
            else
            {
                jsSetting.Formatting = Formatting.None;
            }
            if (setting.IsUsedClassType)
            {
                jsSetting.TypeNameHandling = TypeNameHandling.Auto;
                jsSetting.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full;
            }
            if (setting.IsUseDefaultValue)
            {
                jsSetting.DefaultValueHandling = DefaultValueHandling.Ignore;
            }
            else
            {
                jsSetting.DefaultValueHandling = DefaultValueHandling.Include;
            }
            return jsSetting;
        }
    }
}
