using DotEditor.GUIExtension;
using UnityEditor;

namespace DotEditor.NativeDrawer
{
    public class NativeDrawerEditor : Editor
    {
        private NativeDrawerObject drawerObject = null;

        void OnEnable()
        {
            drawerObject = new NativeDrawerObject(target)
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
            NativeDrawerSetting.OnDrawSetting();

            EGUI.BeginLabelWidth(GetLabelWidth());
            {
                drawerObject.OnGUILayout();
            }
            EGUI.EndLableWidth();
        }
    }
}
