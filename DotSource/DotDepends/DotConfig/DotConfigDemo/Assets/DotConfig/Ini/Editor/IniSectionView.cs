using DotEngine.Config.Ini;
using DotEngine.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace DotEditor.Config.Ini
{
    public class IniSectionView : VisualElement
    {
        private const string DETAIL_UXML = "dot_config_ini_section_detail_uxml";

        [Q("section-name-label")]
        private Label m_NameLabel;
        [Q("comment-textfield")]
        private TextField m_CommentTextField;
        [Q("add-property-btn")]
        private Button m_AddPropertyBtn;

        public IniSectionView()
        {
            this.SetHeight(100, LengthUnit.Percent);
            this.SetWidth(100, LengthUnit.Percent);

            var visualTreeAsset = Resources.Load<VisualTreeAsset>(DETAIL_UXML);
            var visualTree = visualTreeAsset.CloneTree();
            visualTree.SetHeight(100, LengthUnit.Percent);
            visualTree.SetWidth(100, LengthUnit.Percent);
            Add(visualTree);

        }
    }
}
