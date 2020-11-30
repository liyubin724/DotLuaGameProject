using DotEditor.GUIExtension;
using UnityEditor;

namespace DotEditor.NativeDrawer
{
    public class DrawerEditor : Editor
    {
        private DrawerObject drawerObject = null;

        void OnEnable()
        {
            drawerObject = new DrawerObject(target)
            {
                IsShowScroll = IsShowScroll(),
            };
        }

        protected virtual bool IsShowScroll()
        {
            return true;
        }

        protected virtual float GetLabelWidth()
        {
            return 120;
        }

        public override void OnInspectorGUI()
        {
            EGUILayout.DrawScript(target);
            DrawerSetting.OnDrawSetting();

            EGUI.BeginLabelWidth(GetLabelWidth());
            {
                drawerObject.OnGUILayout();
            }
            EGUI.EndLableWidth();
        }
    }
}
