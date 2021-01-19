using DotEditor.GUIExt.Layout;
using DotEditor.GUIExt.NativeDrawer;
using UnityEditor;

namespace DotEditor.GUIExt.EditDrawer
{
    class NativeData
    {
        public int intValue;
        public float floatValue;
        public string stringValue;
        public bool boolValue;
    }
    public class EditDrawerWindow : EditorWindow
    {
        [MenuItem("Test/Edit Drawer Win")]
        static void ShowWin()
        {
            var win = EditorWindow.GetWindow<EditDrawerWindow>();
            win.Show();
        }

        private NativeObjectDrawer nativeObject = null;
        private void OnEnable()
        {
            nativeObject = new NativeObjectDrawer(new NativeData());
        }


        private void OnGUI()
        {
            nativeObject.OnGUILayout();
        }
    }
}
