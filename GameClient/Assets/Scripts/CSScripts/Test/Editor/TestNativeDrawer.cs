using DotEditor.NativeDrawer;
using DotEngine.NativeDrawer.Decorator;
using DotEngine.NativeDrawer.Layout;
using DotEngine.NativeDrawer.Listener;
using DotEngine.NativeDrawer.Property;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using SpaceAttribute = DotEngine.NativeDrawer.Decorator.SpaceAttribute;

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

    public class BaseAttrTestData : BaseTestData
    {
        [EnumButton]
        public TestEnum enumButton;
        [MultilineText]
        public string multilineText;
        [FloatSlider(0, 100)]
        public float floatSlider;
        [FloatSlider("GetFloatSliderLeftValue", "GetFloatSliderRightValue")]
        public float floatSliderMethod;
        [IntSlider(0, 10)]
        public int intSlider;
        [IntSlider("GetIntSliderLeftValue", "GetIntSliderRightValue")]
        public int intSliderMethod;
        [IntPopup(new string[] { "One", "Two", "Three", "Four" }, new int[] { 1, 2, 3, 4 })]
        public int intPopup;
        [StringPopup(new string[] { "Yestaday", "Today", "Tomorrow" })]
        public string stringPopup;
        [OpenFilePath]
        public string openFilePath;
        [OpenFolderPath]
        public string openFolderPath;

        public float GetFloatSliderLeftValue()
        {
            return 10.0f;
        }

        public float GetFloatSliderRightValue()
        {
            return 20.0f;
        }

        public int GetIntSliderLeftValue()
        {
            return 5;
        }

        public int GetIntSliderRightValue()
        {
            return 14;
        }
    }

    public class DecoratorTestData : BaseAttrTestData
    {
        [Space]
        [BoxedHeader("Boxed Header")]
        public int boxedHeader;
        [Button("OnButtonClicked")]
        public int button;
        [SeparatorLine]
        [Help("Help message",HelpMessageType.Warning)]
        public int help;


        public void OnButtonClicked()
        {
            Debug.Log("OnButtonClicked");
        }
    }

    public class LayoutTestData : DecoratorTestData
    {
        [BeginGroup("Begin Group")]
        [EndGroup]
        public int groupValue;

        [BeginHorizontal]
        public int beginHorizontal1;
        public int beginHorizontal2;
        [EndHorizontal]
        public int beginHorizontal3;

        [BeginIndent]
        [EndIndent]
        public int beginIndent;
    }

    public class ListenerTestData
    {
        [OnValueChanged("OnIntValueChanged")]
        public int intValue;

        private void OnIntValueChanged()
        {
            Debug.Log("OnIntValueChanged:value = " + intValue);
        }
    }

    public class TestData : LayoutTestData
    {
        public ListenerTestData ListenerTestData = new ListenerTestData();
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
