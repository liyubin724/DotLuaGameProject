using UnityEditor;

namespace DotEditor.GUIExt.IMGUI
{
    public class MultilineTextDrawer : ValueProviderLayoutDrawable<string>
    {
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
                    Height = EditorGUIUtility.singleLineHeight * LineCount;
                }
            }
        }

        public MultilineTextDrawer()
        {
            Height = EditorGUIUtility.singleLineHeight * LineCount;
        }

        protected override void OnLayoutDraw()
        {
            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.LabelField(Label);
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
