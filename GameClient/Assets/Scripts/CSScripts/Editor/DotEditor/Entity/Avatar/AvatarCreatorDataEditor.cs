using DotEditor.NativeDrawer;
using UnityEditor;

namespace DotEditor.Entity.Avatar
{
    [CustomEditor(typeof(AvatarCreatorData))]
    public class AvatarCreatorDataEditor : NativeDrawerEditor
    {
        protected override bool IsShowScroll()
        {
            return false;
        }
    }
}
