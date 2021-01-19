using UnityEditor;

namespace DotEditor.GUIExt.NativeDrawer
{
    internal class NSpaceDrawer : NLayoutDrawer
    {
        public float Width { get; set; } = 3.0f;
        public bool IsExpand { get; set; } = true;

        public override void OnGUILayout()
        {
            if(Width>0)
            {
                EditorGUILayout.Space(Width, IsExpand);
            }
        }
    }
}
