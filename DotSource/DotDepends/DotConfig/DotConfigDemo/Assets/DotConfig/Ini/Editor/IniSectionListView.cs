using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotEngine.UIElements;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using DotEngine.Config.Ini;
using UnityEngine;

namespace DotEditor.Config.Ini
{
    public class IniSectionListView : VisualElement
    {
        private const string SECTION_LIST_UXML = "dot_config_ini_section_list_uxml";

        [Q("list-add-btn")]
        private Button m_ListAddBtn;
        [Q("section-listview")]
        private ListView m_ListView;

        private IniConfig configData;
        public IniConfig BindedData
        {
            get
            {
                return configData;
            }
            set
            {
                if(value!=configData)
                {
                    configData = value;
                    RefreshView();
                }
            }
        }

        public IniSectionListView()
        {
            this.SetHeight(100, LengthUnit.Percent);
            this.SetWidth(100, LengthUnit.Percent);

            var visualTreeAsset = Resources.Load<VisualTreeAsset>(SECTION_LIST_UXML);
            var visualTree = visualTreeAsset.CloneTree();
            visualTree.SetWidth(100, LengthUnit.Percent);
            visualTree.SetHeight(100, LengthUnit.Percent);
            Add(visualTree);

            visualTree.AssignQueryResults(this);

            m_ListAddBtn.clicked += () =>
            {

            };

        }

        private void RefreshView()
        {
            m_ListView.Clear();
            m_ListView.Refresh();
            
        }
    }
}
