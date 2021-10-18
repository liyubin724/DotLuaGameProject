using DotEditor.GUIExtension;
using DotEngine.NativeDrawer.Decorator;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace DotEditor.NativeDrawer.Decorator
{
    [Binder(typeof(ButtonAttribute))]
    public class ButtonDrawer : DecoratorDrawer
    {
        public override void OnGUILayout()
        {
            var attr = GetAttr<ButtonAttribute>();
            if (string.IsNullOrEmpty(attr.MethodName))
            {
                EGUI.BeginGUIColor(Color.red);
                {
                    EditorGUILayout.LabelField("The name of the method can't be empty.");
                }
                EGUI.EndGUIColor();
            }
            else
            {
                string btnContentStr = string.IsNullOrEmpty(attr.Label) ? attr.MethodName : attr.Label;
                float height = EGUIUtility.singleLineHeight;
                if (attr.Size == ButtonSize.Big)
                {
                    height *= 2;
                }
                else if (attr.Size == ButtonSize.Normal)
                {
                    height *= 1.5f;
                }
                if (GUILayout.Button(btnContentStr, GUILayout.Height(height)))
                {
                    MethodInfo methodInfo = Property.TargetType.GetMethod(attr.MethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (methodInfo != null)
                    {
                        methodInfo.Invoke(Property.Target, null);
                    }
                }
            }
        }
    }
}
