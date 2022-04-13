using DotEditor.GUIExt.IMGUI;
using DotEngine.GUIExt.NativeDrawer;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.NativeDrawer
{
    public abstract class ContentAttrDrawer : ILayoutDrawable, IAttrDrawer
    {
        public DrawerAttribute Attr { get; set; }
        public ItemDrawer ItemDrawer { get; set; }

        public T GetAttr<T>() where T : DrawerAttribute
        {
            return (T)Attr;
        }

        public void OnGUILayout()
        {
            if(IsValidValueType())
            {
                DrawContent();
            }else
            {
                DrawInvalidContent();
            }
        }

        protected abstract bool IsValidValueType();

        protected abstract void DrawContent();

        protected virtual void DrawInvalidContent()
        {
            EGUI.BeginGUIColor(Color.red);
            {
                EditorGUILayout.LabelField(ItemDrawer.Label, "Invalid");
            }
            EGUI.EndGUIColor();
        }
    }
}
