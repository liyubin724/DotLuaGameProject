using System.Collections.Generic;

namespace DotEngine.Config.Ini
{
    public class Property : IDeepCopy<Property>
    {
        private string key;
        public string Key
        {
            get
            {
                return key;
            }
        }

        private string value;
        public bool BoolValue
        {
            get
            {
                if(bool.TryParse(value,out var result))
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
                if(int.TryParse(value,out var result))
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
                if(float.TryParse(value,out var result))
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

        private List<string> comments = null;
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
                if (value == null)
                {
                    comments?.Clear();
                }
                else
                {
                    if (comments == null)
                    {
                        comments = new List<string>();
                    }
                    comments.Clear();
                    comments.AddRange(value);
                }
            }
        }

        private List<string> optionalValues = null;
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
                if (value == null)
                {
                    optionalValues?.Clear();
                }
                else
                {
                    if (optionalValues == null)
                    {
                        optionalValues = new List<string>();
                    }
                    optionalValues.Clear();
                    optionalValues.AddRange(value);
                }
            }
        }

        public Property(string key, string value = "")
        {
            this.key = key;
            this.value = value;
        }

        internal Property(Property p)
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

        public Property DeepCopy()
        {
            return new Property(this);
        }
    }
}
