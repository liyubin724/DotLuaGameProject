using DotEditor.GUIExtension;
using DotEditor.NativeDrawer;
using DotEngine.Lua.Register;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Register
{
    [CustomEditor(typeof(ObjectBindBehaviour),true)]
    public class ObjectBindBehaviourEditor : ScriptBindBehaviourEditor
    {
        ObjectBindBehaviour bindBehaviour = null;
        NativeDrawerObject objectDataDrawer = null;
        protected override void OnEnable()
        {
            base.OnEnable();
            bindBehaviour = target as ObjectBindBehaviour;
            objectDataDrawer = new NativeDrawerObject(bindBehaviour.registerObjectData);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EGUILayout.DrawHorizontalLine();
            objectDataDrawer.OnGUILayout();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
