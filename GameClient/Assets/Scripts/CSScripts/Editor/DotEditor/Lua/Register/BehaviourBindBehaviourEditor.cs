using DotEditor.GUIExtension;
using DotEditor.NativeDrawer;
using DotEngine.Lua.Register;
using UnityEditor;
using UnityEngine;

namespace DotEditor.Lua.Register
{
    [CustomEditor(typeof(BehaviourBindBehaviour),true)]
    public class BehaviourBindBehaviourEditor : ScriptBindBehaviourEditor
    {
        BehaviourBindBehaviour bindBehaviour;
        NativeDrawerObject behaviourDataDrawer = null;

        protected override void OnEnable()
        {
            base.OnEnable();
            bindBehaviour = target as BehaviourBindBehaviour;
            behaviourDataDrawer = new NativeDrawerObject(bindBehaviour.registerBehaviourData);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EGUILayout.DrawHorizontalLine();
            behaviourDataDrawer.OnGUILayout();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }
    }
}
