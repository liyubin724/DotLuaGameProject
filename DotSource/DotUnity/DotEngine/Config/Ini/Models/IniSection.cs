using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Config.Ini
{
    public class IniSection : IDeepCopy<IniSection>, IEnumerable<IniProperty>
    {
        private string name;
        public string Name => name;

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

        private Dictionary<string, IniProperty> properties;

        public string[] PropertyKeys => properties.Keys.ToArray();

        public int PropertyCount => properties.Count;

        public IniProperty this[string propertyKey]
        {
            get
            {
                if (properties.TryGetValue(propertyKey, out var property))
                {
                    return property;
                }
                return null;
            }
        }

        public IniSection(string name)
        {
            this.name = name;
            properties = new Dictionary<string, IniProperty>();
        }

        public IniSection(IniSection section) : this(section.Name)
        {
            name = section.Name;
            if(section.comments!=null)
            {
                Comments = section.comments;
            }
            foreach(var property in section)
            {
                if (properties.ContainsKey(property.Key))
                {
                    properties[property.Key] = property.DeepCopy();
                }
                else
                {
                    properties.Add(property.Key, property.DeepCopy());
                }
            }
        }

        public bool ContainsProperty(string propertyKey) => properties.ContainsKey(propertyKey);
        public IniProperty AddProperty(string propertyKey,string propertyValue,string[] comments = null, string[] optionalValues = null)
        {
            if (properties.ContainsKey(propertyKey))
            {
                return null;
            }
            IniProperty property = new IniProperty(propertyKey, propertyValue);
            if (property != null)
            {
                if(comments!=null && comments.Length>0)
                {
                    property.Comments.AddRange(comments);
                }
                if(optionalValues != null && optionalValues.Length>0)
                {
                    property.OptionalValues.AddRange(optionalValues);
                }
            }
            properties.Add(propertyKey, property);
            return property;
        }

        public IniProperty RemoveProperty(string propertyKey)
        {
            if (properties.TryGetValue(propertyKey, out var property))
            {
                properties.Remove(propertyKey);
                return property;
            }

            return null;
        }

        public IniProperty GetProperty(string propertyKey,bool isCreateIfNExist = false)
        {
            if (!properties.TryGetValue(propertyKey, out var property) && isCreateIfNExist)
            {
                property = AddProperty(propertyKey, string.Empty);
            }

            return property;
        }

        public void ClearProperties()
        {
            properties.Clear();
        }

        public void ClearComments()
        {
            comments?.Clear();
            foreach(IniProperty property in this)
            {
                property.Comments = null;
            }
        }

        public IniSection DeepCopy()
        {
            return new IniSection(this);
        }

        public IEnumerator<IniProperty> GetEnumerator()
        {
            foreach (string key in properties.Keys)
                yield return properties[key];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return properties.GetEnumerator();
        }
    }
}
