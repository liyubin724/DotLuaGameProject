using System.Collections.Generic;

namespace DotEngine.Config.Ini
{
    public class IniProperty : IDeepCopy<IniProperty>
    {
        public string Key { get; private set; }
        public string Value { get; private set; }

        public List<string> Comments { get; set; } = new List<string>();
        public List<string> OptionalValues { get; set; } = new List<string>();

        public string StringValue
        {
            get
            {
                return Value;
            }
            set
            {
                Value = value;
            }
        }

        public bool BoolValue
        {
            get
            {

                if (bool.TryParse(Value, out var result))
                {
                    return result;
                }
                return false;
            }
            set
            {
                Value = value.ToString();
            }
        }

        public int IntValue
        {
            get
            {
                if (int.TryParse(Value, out var result))
                {
                    return result;
                }
                return 0;
            }
            set
            {
                Value = value.ToString();
            }
        }

        public float FloatValue
        {
            get
            {
                if (float.TryParse(Value, out var result))
                {
                    return result;
                }
                return 0.0f;
            }
            set
            {
                Value = value.ToString();
            }
        }

        public IniProperty(string key, string value = "")
        {
            Key = key;
            Value = value;
        }

        public IniProperty(IniProperty p)
        {
            Key = p.Key;
            Value = p.Value;

            Comments.AddRange(p.Comments);
            OptionalValues.AddRange(p.OptionalValues);
        }

        public IniProperty DeepCopy()
        {
            return new IniProperty(this);
        }
    }
}
