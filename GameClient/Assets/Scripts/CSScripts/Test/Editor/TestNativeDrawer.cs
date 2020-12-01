using DotEditor.NativeDrawer;
using DotEngine.NativeDrawer.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace TestEditor
{
    public enum TestEnum
    {
        One,
        Two,
        Three,
        Four,
        Five,
    }

    public class BaseTestData
    {
        public int intValue;
        public float floatValue;
        public string strValue;
        public bool boolValue;
        public Bounds boundsValue;
        public Rect rectValue;
        public Vector2 vector2Value;
        public Vector3 vector3Value;
        public TestEnum testEnum;
        public GameObject gObjValue;
    }

    public class TestData : BaseTestData
    {
        [EnumButton]
        public TestEnum enumButton;
        [MultilineText]
        public string multilineText;
        [FloatSlider(0,100)]
        public float floatSlider;
        [IntSlider(0,10)]
        public int intSlider;
        [IntPopup(new string[] { "One", "Two", "Three", "Four" }, new int[] { 1, 2, 3, 4 })]
        public int intPopup;
        [OpenFilePath]
        public string openFilePath;
        [OpenFolderPath]
        public string openFolderPath;
    }

    public class TestNativeDrawer : EditorWindow
    {

        [MenuItem("Test/Test Native Drawer")]
        static void ShowWin()
        {
            var win = GetWindow<TestNativeDrawer>();
            win.Show();
        }

        TestData testData = new TestData();

        DrawerObject drawerObj = null;

        private void OnEnable()
        {
            drawerObj = new DrawerObject(testData)
            {
                IsShowScroll = true,
                IsShowInherit = true,
            };
        }
       private void OnGUI()
        {
            drawerObj.OnGUILayout();
        }

    }
}
