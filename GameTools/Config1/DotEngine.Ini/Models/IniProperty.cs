using System.Collections.Generic;

namespace DotEngine.Ini
{
    public class IniProperty : IDeepCopy<IniProperty>
    {
        private string key;
        private string value = "";
        private List<string> comments = null;
        private List<string> optionalValues = null;

        public string Key
        {
            get
            {
                return key;
            }
        }

        public string StringValue
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }

        public bool BoolValue
        {
            get
            {

                if (bool.TryParse(value, out var result))
                {
                    return result;
                }
                return false;
            }
            set
            {
                this.value = value.ToString();
            }
        }

        public int IntValue
        {
            get
            {
                if (int.TryParse(value, out var result))
                {
                    return result;
                }
                return 0;
            }
            set
            {
                this.value = value.ToString();
            }
        }

        public float FloatValue
        {
            get
            {
                if (float.TryParse(value, out var result))
                {
                    return result;
                }
                return 0.0f;
            }
            set
            {
                this.value = value.ToString();
            }
        }

        public List<string> Comments
        {
            get
            {
                if (comments == null)
                {
                    comments = new List<string>();
                }
                return comments;
            }
            set
            {
                comments?.Clear();
                if (value != null)
                {
                    if (comments == null)
                    {
                        comments = new List<string>();
                    }
                    comments.AddRange(value);
                }
            }
        }

        public List<string> OptionalValues
        {
            get
            {
                if (optionalValues == null)
                {
                    optionalValues = new List<string>();
                }
                return optionalValues;
            }
            set
            {
                optionalValues?.Clear();
                if (value != null)
                {
                    if (optionalValues == null)
                    {
                        optionalValues = new List<string>();
                    }
                    optionalValues.AddRange(value);
                }
            }
        }

        public IniProperty(string key, string value = "")
        {
            this.key = key;
            this.value = value;
        }

        internal IniProperty(IniProperty p)
        {
            key = p.Key;
            value = p.StringValue;
            if (p.comments != null)
            {
                Comments = p.Comments;
            }
            if (p.optionalValues != null)
            {
                OptionalValues = p.OptionalValues;
            }
        }

        public IniProperty DeepCopy()
        {
            return new IniProperty(this);
        }
    }
}
