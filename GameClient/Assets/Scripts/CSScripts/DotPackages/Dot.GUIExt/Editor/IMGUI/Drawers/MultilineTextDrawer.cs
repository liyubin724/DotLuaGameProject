using UnityEditor;

namespace DotEditor.GUIExt.IMGUI
{
    public class MultilineTextDrawer : ValueProviderLayoutDrawable<string>
    {
        private bool isLineCountChanged = false;

        private int lineCount = 4;
        public int LineCount
        {
            get
            {
                return lineCount;
            }
            set
            {
                if(lineCount!=value)
                {
                    lineCount = value;
                    isLineCountChanged = true;
                }
            }
        }

        protected override void OnLayoutDraw()
        {
           if(isLineCountChanged)
            {
                Width = EditorGUIUtility.singleLineHeight * LineCount;
                isLineCountChanged = false;
            }

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.PrefixLabel(Label);
                EGUI.BeginIndent();
                {
                    Value = EditorGUILayout.TextArea(Value, LayoutOptions);
                }
                EGUI.EndIndent();
            }
            EditorGUILayout.EndVertical();
        }
    }
}
