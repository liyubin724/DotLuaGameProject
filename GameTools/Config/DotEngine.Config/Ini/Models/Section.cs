using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Config.Ini
{
    public class Section : IDeepCopy<Section>, IEnumerable<Property>
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
                if (comments == null)
                {
                    comments = new List<string>();
                }
                comments.Clear();
                if (value != null)
                {
                    comments.AddRange(value);
                }
            }
        }

        private Dictionary<string, Property> properties;

        public string[] PropertyKeys => properties.Keys.ToArray();

        public int PropertyCount => properties.Count;

        public Property this[string propertyKey]
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

        public Section(string name)
        {
            this.name = name;
            properties = new Dictionary<string, Property>();
        }

        public Section(Section section) : this(section.Name)
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
        public Property AddProperty(string propertyKey,string propertyValue,string[] comments = null, string[] optionalValues = null)
        {
            if (properties.ContainsKey(propertyKey))
            {
                return null;
            }
            Property property = new Property(propertyKey, propertyValue);
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
            return property;
        }

        public Property RemoveProperty(string propertyKey)
        {
            if (properties.TryGetValue(propertyKey, out var property))
            {
                properties.Remove(propertyKey);
                return property;
            }

            return null;
        }

        public Property GetProperty(string propertyKey,bool isCreateIfNExist = false)
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
            foreach(Property property in this)
            {
                property.Comments = null;
            }
        }

        public Section DeepCopy()
        {
            return new Section(this);
        }

        public IEnumerator<Property> GetEnumerator()
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
