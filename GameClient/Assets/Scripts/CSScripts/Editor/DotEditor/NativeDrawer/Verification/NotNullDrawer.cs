using DotEngine.NativeDrawer;
using DotEngine.NativeDrawer.Verification;
using UnityEditor;

namespace DotEditor.NativeDrawer.Verification
{
    [AttrDrawBinder(typeof(NotNullAttribute))]
    public class NotNullDrawer : VerificationDrawer
    {
        public NotNullDrawer(object target, VerificationCompareAttribute attr) : base(target, attr)
        {
        }

        public override void OnGUILayout()
        {
            NotNullAttribute attr = GetAttr<NotNullAttribute>();

            //bool isNotNull = IsValid();
            //if(!isNotNull)
            //{
            //    EditorGUILayout.HelpBox(string.IsNullOrEmpty(attr.UnvalidMsg) ? "" : attr.UnvalidMsg, MessageType.Error);
            //}
        }
    }
}
