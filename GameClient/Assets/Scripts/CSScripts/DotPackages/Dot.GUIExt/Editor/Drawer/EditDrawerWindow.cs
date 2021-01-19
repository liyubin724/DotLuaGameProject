using DotEditor.GUIExt.Layout;
using DotEditor.GUIExt.NativeDrawer;
using DotEngine.Utilities;
using System;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.EditDrawer
{
    class BaseData
    {
        public int baseIntValue;
        public int baseFloatValue;
    }

    class SimpleData : BaseData
    {
        public int intValue;
        public float floatValue;
        public string stringValue;
        public bool boolValue;
    }

    class ComposedData : SimpleData
    {
        public Vector3 vector3Value;
    }

    class NativeData : ComposedData
    {
        
    }

    

    enum NativeEnum
    {
        None,
        First,
        Second,
        Third,
    }

    public class EditDrawerWindow : EditorWindow
    {
        [MenuItem("Test/Edit Drawer Win")]
        static void ShowWin()
        {
            var win = EditorWindow.GetWindow<EditDrawerWindow>();
            win.Show();
        }

        private NObjectDrawer nativeObject = null;
        private void OnEnable()
        {
            nativeObject = new NObjectDrawer(new NativeData())
            {
                IsShowInherit = true,
                IsShowScroll = true,
            };

            Type enumType = typeof(NativeEnum);
            Debug.Log(GetTypeInfo(enumType));
        }

        private string GetTypeInfo(Type type)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"IsStructType = {TypeUtility.IsStructType(type)}");
            sb.AppendLine($"IsListType = {TypeUtility.IsListType(type)}");
            sb.AppendLine($"IsArrayType = {TypeUtility.IsArrayType(type)}");
            sb.AppendLine($"IsPrimitiveType = {TypeUtility.IsPrimitiveType(type)}");
            sb.AppendLine($"IsEnumType = {TypeUtility.IsEnumType(type)}");
            return sb.ToString();
        }

        private void OnGUI()
        {
            nativeObject.OnGUILayout();
        }
    }
}
