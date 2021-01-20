using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    internal class NHeadDrawer : NLayoutDrawer
    {
        public string Header { get; set; }
        public bool IsExpandWidth { get; set; } = true;

        public override void OnGUILayout()
        {
            string label = string.IsNullOrEmpty(Header) ? "" : Header;
            GUILayoutOption expandWidthOption = GUILayout.ExpandWidth(IsExpandWidth);
            EGUILayout.DrawBoxHeader(label, expandWidthOption);
        }
    }
}