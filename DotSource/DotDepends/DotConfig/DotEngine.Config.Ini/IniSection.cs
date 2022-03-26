using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Config.Ini
{
    public class IniSection : IDeepCopy<IniSection>, IEnumerable<IniProperty>
    {
        public string Name { get; private set; }
        public List<string> Comments { get; set; } = new List<string>();

        private Dictionary<string, IniProperty> properties = new Dictionary<string, IniProperty>();

        public int PropertyCount => properties.Count;
        public string[] PropertyKeys => properties.Keys.ToArray();

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
            Name = name;
        }

        public IniSection(IniSection section)
        {
            Name = section.Name;
            Comments.AddRange(section.Comments);
            foreach (var property in section)
            {
                properties.Add(property.Key, property.DeepCopy());
            }
        }

        public bool ContainsProperty(string propertyKey) => properties.ContainsKey(propertyKey);

        public IniProperty AddProperty(string propertyKey, string propertyValue, string[] comments = null, string[] optionalValues = null)
        {
            if (properties.ContainsKey(propertyKey))
            {
                throw new IniPropertyRepeatException(propertyKey);
            }

            IniProperty property = new IniProperty(propertyKey, propertyValue);
            if(comments!=null && comments.Length>0)
            {
                property.Comments.AddRange(comments);
            }
            if(optionalValues!=null && optionalValues.Length>0)
            {
                property.OptionalValues.AddRange(optionalValues);
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

        public IniProperty GetProperty(string propertyKey, bool isCreateIfNExist = false)
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
            Comments.Clear();
            foreach (IniProperty property in this)
            {
                property.Comments.Clear();
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
