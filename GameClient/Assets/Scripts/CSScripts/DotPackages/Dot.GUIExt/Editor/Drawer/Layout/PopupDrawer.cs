using System;
using UnityEditor;

namespace DotEditor.GUIExt.Layout
{
    public class PopupDrawer<T> : ValueProviderLayoutDrawable<T>
    {
        public string[] Contents { get; set; } = new string[0];
        public T[] Values { get; set; } = new T[0];

        protected override void OnLayoutDraw()
        {
            int index = Array.IndexOf(Values, Value);
            if (index < 0) index = 0;
            int newIndex = EditorGUILayout.Popup(Label, index, Contents,LayoutOptions);

            if(newIndex>=0)
                Value = Values[newIndex];
        }
    }
}
