using DotEditor.GUIExtension;
using DotEngine.NativeDrawer.Decorator;
using System.Reflection;

namespace DotEditor.NativeDrawer.Decorator
{
    [AttrBinder(typeof(ButtonAttribute))]
    public class ButtonDrawer : DecoratorDrawer
    {
        public override void OnGUILayout()
        {
            var attr = GetAttr<ButtonAttribute>();
            if (string.IsNullOrEmpty(attr.MethodName))
            {
                EGUI.BeginGUIColor(UnityEngine.Color.red);
                {
                    UnityEditor.EditorGUILayout.LabelField("The name of the method can't be empty.");
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
                if (UnityEngine.GUILayout.Button(btnContentStr, UnityEngine.GUILayout.Height(height)))
                {
                    MethodInfo methodInfo = DrawerProperty.Target.GetType().GetMethod(attr.MethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (methodInfo != null)
                    {
                        methodInfo.Invoke(DrawerProperty.Target, null);
                    }
                }
            }
        }
    }
}
