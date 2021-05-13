using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Ini
{
    public class IniData : IDeepCopy<IniData>, IEnumerable<IniSection>
    {
        public static readonly string GLOBAL_SECTION_NAME = "__global__";

        private Dictionary<string, IniSection> sections = null;

        public string[] SectionNames => sections.Keys.ToArray();
        public int SectionCount => sections.Count;

        public IniData()
        {
            sections = new Dictionary<string, IniSection>();
        }

        public IniData(IniData data)
        {
            sections = new Dictionary<string, IniSection>();
            foreach(IniSection section in data)
            {
                sections.Add(section.Name, section.DeepCopy());
            }
        }

        public bool ContainsSection(string sectionName) => sections.ContainsKey(sectionName);
        public IniSection AddSection(string sectionName)
        {
            if(!sections.TryGetValue(sectionName,out var section))
            {
                section = new IniSection(sectionName);
                sections.Add(sectionName, section);
            }
            return section;
        }

        public IniSection RemoveSection(string sectionName)
        {
            if (sections.TryGetValue(sectionName, out var section))
            {
                sections.Remove(sectionName);
            }
            return section;
        }

        public IniSection GetSection(string sectionName,bool isCreateIfNExist = false)
        {
            if (!sections.TryGetValue(sectionName, out var section) && isCreateIfNExist)
            {
                section = new IniSection(sectionName);
                sections.Add(sectionName, section);
            }
            return section;
        }

        public void ClearSections()
        {
            sections.Clear();
        }

        public IniProperty GetProperty(string sectionName,string propertyKey)
        {
            IniSection section = GetSection(sectionName, false);
            if(section!=null)
            {
                return section.GetProperty(propertyKey, false);
            }
            return null;
        }

        public IniProperty GetProperty(string propertyKey)
        {
            foreach(var section in this)
            {
                if(section.ContainsProperty(propertyKey))
                {
                    return section.GetProperty(propertyKey, false);
                }
            }
            return null;
        }

        public IniData DeepCopy()
        {
            return new IniData(this);
        }

        public IEnumerator<IniSection> GetEnumerator()
        {
            foreach(var name in sections.Keys)
            {
                yield return sections[name];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return sections.GetEnumerator();
        }
    }
}
