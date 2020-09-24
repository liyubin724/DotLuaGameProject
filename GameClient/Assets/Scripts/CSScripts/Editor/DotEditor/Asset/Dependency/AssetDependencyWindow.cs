using UnityEditor;
using UnityEngine;

namespace DotEditor.Asset.Dependency
{
    public class IngoreAssetExtension
    {
        public string displayName = "";
        public string extension = "";
        public bool isSelected = false;
    }

    public class AssetDependencyWindow : EditorWindow
    {
        [MenuItem("Game/Asset/Dependency Window", priority = 20)]
        public static void ShowWin()
        {
            AssetDependencyWindow win = EditorWindow.GetWindow<AssetDependencyWindow>();
            win.titleContent = new GUIContent("Bundle Packer");
            win.Show();
        }

        [MenuItem("Game/Asset/Close Dependency Window", priority = 21)]
        public static void CloseWin()
        {
            EditorUtility.ClearProgressBar();
            AssetDependencyWindow win = EditorWindow.GetWindow<AssetDependencyWindow>();
            win.titleContent = new GUIContent("Bundle Packer");
            win.Close();
        }

        private GUIContent[] m_ToolbarContents = new GUIContent[]
        {
            new GUIContent("DependOn"),
            new GUIContent("UsedBy"),
        };
        private int m_ToolbarSelectedIndex = 0;
 
        private AAssetDependencyTab[] m_TabViews = null;
        private void OnEnable()
        {
            m_TabViews = new AAssetDependencyTab[]
            {
                new AssetDependOnTab(this),
                new AssetUsedByTab(this),
            };
            m_TabViews[m_ToolbarSelectedIndex].OnEnable();
        }

        private void OnGUI()
        {
            int selectedIndex = GUILayout.Toolbar(m_ToolbarSelectedIndex, m_ToolbarContents, GUILayout.ExpandWidth(true), GUILayout.Height(40));
            if(selectedIndex != m_ToolbarSelectedIndex)
            {
                m_TabViews[m_ToolbarSelectedIndex].OnDisable();
                m_ToolbarSelectedIndex = selectedIndex;
                m_TabViews[m_ToolbarSelectedIndex].OnEnable();
            }

            m_TabViews[m_ToolbarSelectedIndex].OnGUI();
        }

    }
}
