using DotEditor.GUIExtension;
using UnityEditor;

namespace DotEditor.NativeDrawer
{
    public class DrawerEditor : Editor
    {
        private DrawerObject drawerObject = null;

        protected virtual void OnEnable()
        {
            drawerObject = new DrawerObject(target)
            {
                IsShowScroll = IsShowScroll(),
                IsShowInherit = IsShowInherit(),
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

        protected virtual bool IsShowInherit()
        {
            return true;
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
