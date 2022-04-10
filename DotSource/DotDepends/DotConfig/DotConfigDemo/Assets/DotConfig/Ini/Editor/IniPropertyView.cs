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
    public class IniPropertyView : VisualElement
    {
        private const string PROPERTY_UXML = "dot_config_ini_property_uxml";

        private IniProperty propertyData;
        public IniPropertyView()
        {
            this.SetHeight(100, LengthUnit.Percent);
            this.SetWidth(100, LengthUnit.Percent);

            var visualTreeAsset = Resources.Load<VisualTreeAsset>(PROPERTY_UXML);
            var visualTree = visualTreeAsset.CloneTree();
            visualTree.SetHeight(100, LengthUnit.Percent);
            visualTree.SetWidth(100, LengthUnit.Percent);
            Add(visualTree);
        }
    }
}
