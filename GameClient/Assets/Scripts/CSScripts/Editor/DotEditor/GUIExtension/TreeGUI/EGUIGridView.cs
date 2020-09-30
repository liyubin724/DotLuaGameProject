using DotEditor.GUIExtension.TreeGUI;
using SuperScrollView;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace DotEditor.GUIExtension.GridGUI
{
    public class EGUIGridView
    {
        public TreeViewModel ViewModel { get; private set; }

        public EGUIGridView(TreeViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }

}
