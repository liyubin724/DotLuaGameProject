using DotEngine.NativeDrawer.Verification;
using UnityEditor;

namespace DotEditor.NativeDrawer.Verification
{
    public abstract class VerificationDrawer : Drawer
    {
        public abstract bool IsValid();

        public void OnGUILayout()
        {
            VerificationAttribute attr = GetAttr<VerificationAttribute>();

            if(!IsValid())
            {
                EditorGUILayout.HelpBox(attr.InvalidMsg, MessageType.Error);
            }
        }
    }
}
