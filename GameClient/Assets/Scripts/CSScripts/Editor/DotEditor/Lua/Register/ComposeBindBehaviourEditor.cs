using DotEditor.GUIExtension;
using DotEditor.NativeDrawer;
using DotEngine.Lua.Register;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Register
{
    [CustomEditor(typeof(ComposeBindBehaviour),true)]
    public class ComposeBindBehaviourEditor : ScriptBindBehaviourEditor
    {
        ComposeBindBehaviour bindBehaviour;
        NativeDrawerObject objectDataDrawer = null;
        NativeDrawerObject behaviourDataDrawer = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            bindBehaviour = target as ComposeBindBehaviour;
            objectDataDrawer = new NativeDrawerObject(bindBehaviour.registerObjectData);
            behaviourDataDrawer = new NativeDrawerObject(bindBehaviour.registerBehaviourData);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EGUILayout.DrawHorizontalLine();
            objectDataDrawer.OnGUILayout();

            EGUILayout.DrawHorizontalLine();
            behaviourDataDrawer.OnGUILayout();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
