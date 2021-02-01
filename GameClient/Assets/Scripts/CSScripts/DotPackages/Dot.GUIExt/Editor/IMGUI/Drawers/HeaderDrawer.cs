using UnityEngine;

namespace DotEditor.GUIExt.IMGUI
{
    public class HeaderDrawer : ILayoutDrawable
    {
        public string Header { get; set; }
        public bool IsExpandWidth { get; set; } = true;
        public bool IsLayoutCenter { get; set; } = false;

        public void OnGUILayout()
        {
            string label = string.IsNullOrEmpty(Header) ? "" : Header;
            GUILayoutOption expandWidthOption = GUILayout.ExpandWidth(IsExpandWidth);
            GUIStyle style = IsLayoutCenter ? EGUIStyles.BoxedHeaderCenterStyle : EGUIStyles.BoxedHeaderStyle;
            EGUILayout.DrawBoxHeader(label, style, expandWidthOption);
        }
    }
}
