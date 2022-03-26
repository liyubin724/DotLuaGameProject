using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DotEngine.Config.Ini
{
    public class IniConfig : IDeepCopy<IniConfig>, IEnumerable<IniSection>
    {
        private Dictionary<string, IniSection> sectionDic = new Dictionary<string, IniSection>();

        public int SectionCount => sectionDic.Count;
        public string[] SectionNames => sectionDic.Keys.ToArray();

        public IniConfig()
        {
        }

        public IniConfig(IniConfig data)
        {
            foreach (IniSection section in data)
            {
                sectionDic.Add(section.Name, section.DeepCopy());
            }
        }

        public bool ContainsSection(string sectionName) => sectionDic.ContainsKey(sectionName);
        public IniSection AddSection(string sectionName)
        {
            if (!sectionDic.TryGetValue(sectionName, out var section))
            {
                section = new IniSection(sectionName);
                sectionDic.Add(sectionName, section);
            }
            return section;
        }

        public void AddSection(IniSection section)
        {
            if(sectionDic.ContainsKey(section.Name))
            {
                throw new IniSectionRepeatException(section.Name);
            }
            sectionDic.Add(section.Name, section);
        }

        public IniSection RemoveSection(string sectionName)
        {
            if (sectionDic.TryGetValue(sectionName, out var section))
            {
                sectionDic.Remove(sectionName);
            }
            return section;
        }

        public IniSection GetSection(string sectionName, bool isCreateIfNExist = false)
        {
            if (!sectionDic.TryGetValue(sectionName, out var section) && isCreateIfNExist)
            {
                section = new IniSection(sectionName);
                sectionDic.Add(sectionName, section);
            }
            return section;
        }

        public void ClearSections()
        {
            sectionDic.Clear();
        }

        public IniProperty GetProperty(string sectionName, string propertyKey)
        {
            IniSection section = GetSection(sectionName, false);
            if (section != null)
            {
                return section.GetProperty(propertyKey, false);
            }
            return null;
        }

        public IniProperty GetProperty(string propertyKey)
        {
            foreach (var section in this)
            {
                if (section.ContainsProperty(propertyKey))
                {
                    return section.GetProperty(propertyKey, false);
                }
            }
            return null;
        }

        public IniConfig DeepCopy()
        {
            return new IniConfig(this);
        }

        public IEnumerator<IniSection> GetEnumerator()
        {
            foreach (var name in sectionDic.Keys)
            {
                yield return sectionDic[name];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return sectionDic.GetEnumerator();
        }
    }
}
