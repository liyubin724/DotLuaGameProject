using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Config.Ini
{
    public class IniData : IDeepCopy<IniData>, IEnumerable<Section>
    {
        public static readonly string GLOBAL_SECTION_NAME = "__global__";

        public bool CreateSectionsIfTheyDontExist { get; set; } = false;

        private Dictionary<string, Section> sections = null;

        public string[] SectionNames => sections.Keys.ToArray();
        public int SectionCount => sections.Count;

        public IniData()
        {
            sections = new Dictionary<string, Section>();
        }

        public IniData(IniData data)
        {
            CreateSectionsIfTheyDontExist = data.CreateSectionsIfTheyDontExist;
            sections = new Dictionary<string, Section>();
            foreach(Section section in data)
            {
                sections.Add(section.Name, section.DeepCopy());
            }
        }

        public bool ContainsSection(string sectionName) => sections.ContainsKey(sectionName);
        public Section AddSection(string sectionName)
        {
            if(!sections.TryGetValue(sectionName,out var section))
            {
                section = new Section(sectionName);
                sections.Add(sectionName, section);
            }
            return section;
        }

        public Section RemoveSection(string sectionName)
        {
            if (sections.TryGetValue(sectionName, out var section))
            {
                sections.Remove(sectionName);
            }
            return section;
        }

        public Section GetSection(string sectionName,bool isCreateIfNExist = false)
        {
            if (!sections.TryGetValue(sectionName, out var section) && isCreateIfNExist)
            {
                section = new Section(sectionName);
                sections.Add(sectionName, section);
            }
            return section;
        }

        public void ClearSections()
        {
            sections.Clear();
        }

        public Property GetProperty(string sectionName,string propertyKey)
        {
            Section section = GetSection(sectionName, false);
            if(section!=null)
            {
                return section.GetProperty(propertyKey, false);
            }
            return null;
        }

        public Property GetProperty(string propertyKey)
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

        public IEnumerator<Section> GetEnumerator()
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
