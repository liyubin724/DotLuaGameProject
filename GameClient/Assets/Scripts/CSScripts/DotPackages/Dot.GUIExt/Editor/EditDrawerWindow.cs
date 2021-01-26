using DotEditor.GUIExt.IMGUI;
using DotEditor.GUIExt.NativeDrawer;
using DotEngine.GUIExt.NativeDrawer;
using DotEngine.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace DotEditor.GUIExt.EditDrawer
{
    enum NativeEnum
    {
        None,
        First,
        Second,
        Third,
    }

    [Flags]
    enum NativeFlagEnum
    {
        None = 1<<0,
        First = 1<<1,
        Second = 1<<2,
        Third = 1<<3,
    }

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

        [EnumButton]
        public NativeEnum enumValue = NativeEnum.First;
        public NativeFlagEnum flagEnumValue = NativeFlagEnum.First | NativeFlagEnum.Second;
    }

    class ArrayData : SimpleData
    {
        public int[] intArrValue;
        public List<BaseData> intListValue;
    }

    class ComposedData : ArrayData
    {
        public Vector3 vector3Value;
        public Vector2 vector2Value;
        public Rect rectValue;
        public Bounds boundsValue;

        public Action actionValue;
    }

    class NativeData// : ComposedData
    {
        [EnumButton]
        public NativeEnum enumValue = NativeEnum.First;
        [EnumButton]
        public NativeFlagEnum flagEnumValue = NativeFlagEnum.First | NativeFlagEnum.Second;
        [FloatSlider(0,100)]
        public float floatValue;

        [Hide]
        public int hideIntValue;
        [Show]
        private int showIntValue;
        [VisibleIf("IsVisible")]
        public int visibleIntValue;
        [CompareVisible("GetComparedValue",10,CompareSymbol.Gt)]
        public int comparedIntValue;

        private bool IsVisible()
        {
            return floatValue >= 50;
        }

        private float GetComparedValue()
        {
            return floatValue;
        }

        //public IBaseValue iBaseValue;
        //public List<IBaseValue> baseValues = new List<IBaseValue>()
        //{
        //    new TDValue(),
        //    new TDValue(),
        //    new TDValue(),
        //    new TDValue(),
        //};
    }

    public interface IBaseValue
    {

    }

    public class TDValue : IBaseValue
    {
        public int intValue;
    }

    

    public class EditDrawerWindow : EditorWindow
    {
        [MenuItem("Test/Edit Drawer Win")]
        static void ShowWin()
        {
            var win = EditorWindow.GetWindow<EditDrawerWindow>();
            win.Show();
        }

        private ObjectDrawer nativeObject = null;
        private void OnEnable()
        {
            NativeData nData = new NativeData();
           // nData.iBaseValue = new TDValue();

            nativeObject = new ObjectDrawer(nData)
            {
                IsShowInherit = true,
                IsShowScroll = true,
                IsShowBox = true,
                Header = "Test For NativeData"
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

        private ToolbarDrawer toolbarDrawer = null;
        private EasyListViewDrawer listViewDrawer = null;
        private Rect contentRect;
        private string selectedFilePath = null;

        private DragLineDrawer dragLineDrawer = null;
        private bool isInited = false;

        private EnumButtonDrawer enumButtonDrawer = null;
        private void OnGUI2()
        {
            if (enumButtonDrawer == null)
            {
                enumButtonDrawer = new EnumButtonDrawer(typeof(NativeFlagEnum));
                enumButtonDrawer.Value = NativeFlagEnum.First | NativeFlagEnum.Second;
                enumButtonDrawer.IsExpandWidth = true;
                enumButtonDrawer.OnValueChanged = (value) =>
                {
                    Debug.Log("" + value);
                };
            }
            enumButtonDrawer.OnGUILayout();
        }

        private void OnGUI()
        {
            

            if(toolbarDrawer == null)
            {
                BaseData[] datas = new BaseData[]
                {
                    new BaseData(),
                    new BaseData(),
                };
                string[] contents2 = new string[]
                {
                    "BaseData 1",
                    "BaseData 2"
                };

                toolbarDrawer = new ToolbarDrawer()
                {
                    LeftDrawable = new HorizontalLayoutDrawer(new ToolbarButtonDrawer()
                    {
                        Text = "Open",
                        Tooltip = "open a new file",
                        OnClicked = () =>
                        {
                            Debug.Log("ToolbarButton->Clicked");
                        }
                    },
                    new PopupDrawer<BaseData>()
                    {
                        Values = datas,
                        Value = datas[0],
                        Contents = contents2,
                    }),
                    RightDrawable = new HorizontalLayoutDrawer(
                        new ToolbarToggleDrawer()
                        {
                            Text = "Auto Save",
                            OnValueChanged = (isSelected) =>
                            {
                                Debug.Log("ToolbarToggleDrawer->isSelected = " + isSelected);
                            }
                        }, 
                        new SearchFieldDrawer()
                        {
                            OnValueChanged = (searchText) =>
                            {
                                Debug.Log("SearchFieldDrawer->searchText = " + searchText);
                            }
                        }),
                };
            }
            toolbarDrawer.OnGUILayout();

            if(listViewDrawer ==  null)
            {
                listViewDrawer = new EasyListViewDrawer()
                {
                    Text = "List View",
                    DisplayNames = (from file in Directory.GetFiles(@"D:\", "*.*", SearchOption.TopDirectoryOnly) select Path.GetFileNameWithoutExtension(file)).ToArray(),
                    Values = (from file in Directory.GetFiles(@"D:\", "*.*", SearchOption.TopDirectoryOnly) select file).ToArray(),
                    OnValueChanged = (file) =>
                    {
                        selectedFilePath = (string)file;
                    }
                };
            }
            if(dragLineDrawer == null)
            {
                dragLineDrawer = new DragLineDrawer(this, DragLineDirection.Vertical);
            }
            Rect cRect = EditorGUILayout.GetControlRect(GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
            if(cRect!=contentRect && cRect.width >10&&cRect.height>10)
            {
                contentRect = cRect;
                dragLineDrawer.Position = new Rect(contentRect.x + 300, contentRect.y, 6, contentRect.height);
            }
            dragLineDrawer.LowLimitXY = 200;
            dragLineDrawer.UpperLimitXY = contentRect.width * 0.8f;
            dragLineDrawer.OnGUILayout();

            Rect listViewRect = new Rect(contentRect.x, contentRect.y, dragLineDrawer.MinX - contentRect.x, contentRect.height);
           // EGUI.DrawAreaLine(listViewRect, Color.blue);
            GUILayout.BeginArea(listViewRect);
            {
                listViewDrawer.OnGUILayout();
            }
            GUILayout.EndArea();

            Rect objectRect = new Rect(dragLineDrawer.MaxX, contentRect.y, contentRect.width - dragLineDrawer.MaxX, contentRect.height);
          //  EGUI.DrawAreaLine(objectRect, Color.yellow);
            GUILayout.BeginArea(objectRect);
            {
                if (string.IsNullOrEmpty(selectedFilePath))
                {
                    nativeObject.OnGUILayout();
                }
                else
                {
                    GUILayout.TextField(File.ReadAllText(selectedFilePath), GUILayout.ExpandHeight(true), GUILayout.ExpandWidth(true));
                }
            }
            GUILayout.EndArea();

        }
    }
}
