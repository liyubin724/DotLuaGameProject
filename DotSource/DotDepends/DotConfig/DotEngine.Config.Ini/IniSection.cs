using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Config.Ini
{
    public class IniSection : IDeepCopy<IniSection>, IEnumerable<IniProperty>
    {
        public string Name { get; internal set; }
        public List<string> Comments { get; set; } = new List<string>();

        private Dictionary<string, IniProperty> propertyDic = new Dictionary<string, IniProperty>();

        public int PropertyCount => propertyDic.Count;
        public string[] PropertyKeys => propertyDic.Keys.ToArray();

        public IniProperty this[string propertyKey]
        {
            get
            {
                if (propertyDic.TryGetValue(propertyKey, out var property))
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
                propertyDic.Add(property.Key, property.DeepCopy());
            }
        }

        public bool ContainsProperty(string propertyKey) => propertyDic.ContainsKey(propertyKey);

        public IniProperty AddProperty(string propertyKey, string propertyValue, string[] comments = null, string[] optionalValues = null)
        {
            if (propertyDic.ContainsKey(propertyKey))
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

            propertyDic.Add(propertyKey, property);

            return property;
        }

        public void AddProperty(IniProperty property)
        {
            if (propertyDic.ContainsKey(property.Key))
            {
                throw new IniPropertyRepeatException(property.Key);
            }
            propertyDic.Add(property.Key, property);
        }

        public IniProperty RemoveProperty(string propertyKey)
        {
            if (propertyDic.TryGetValue(propertyKey, out var property))
            {
                propertyDic.Remove(propertyKey);

                return property;
            }

            return null;
        }

        public IniProperty GetProperty(string propertyKey, bool isCreateIfNExist = false)
        {
            if (!propertyDic.TryGetValue(propertyKey, out var property) && isCreateIfNExist)
            {
                property = AddProperty(propertyKey, string.Empty);
            }

            return property;
        }

        public void ClearProperties()
        {
            propertyDic.Clear();
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
            foreach (string key in propertyDic.Keys)
                yield return propertyDic[key];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return propertyDic.GetEnumerator();
        }
    }
}
