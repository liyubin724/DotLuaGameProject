using DotEditor.GUIExt.IMGUI;
using DotEngine.GUIExt.NativeDrawer;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    public abstract class ContentAttrDrawer : LayoutDrawable, INAttrDrawer
    {
        public NDrawerAttribute DrawerAttr { get; set; }
        internal NItemDrawer ItemDrawer { get; set; }

        public T GetAttr<T>() where T : NDrawerAttribute
        {
            return (T)DrawerAttr;
        }
        protected abstract bool IsValidValueType();

        protected override void OnLayoutDraw()
        {
            if(IsValidValueType())
            {
                DrawContent();
            }else
            {
                DrawInvalidContent();
            }
        }

        protected abstract void DrawContent();

        protected virtual void DrawInvalidContent()
        {
            EGUI.BeginGUIColor(Color.red);
            {
                EditorGUILayout.LabelField(Label, "Invalid");
            }
            EGUI.EndGUIColor();
        }
    }
}
