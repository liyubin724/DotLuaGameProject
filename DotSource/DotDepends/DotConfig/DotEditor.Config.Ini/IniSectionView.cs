using DotEngine.Config.Ini;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

namespace DotEditor.Config.Ini
{
    public class IniSectionView : VisualElement
    {
        private TextField m_NameField;
        private TextField m_CommentField;

        private ListView m_PropertyListView;

        private IniSection m_SectionData;
        public IniSection BindData
        {
            get
            {
                return m_SectionData;
            }
            set
            {
                if(m_SectionData!=value)
                {
                    m_SectionData = value;
                }
            }
        }

        public IniSectionView()
        {
            m_NameField = new TextField("Name:");
            m_NameField.RegisterValueChangedCallback((e) =>
            {

            });
        }
    }
}
