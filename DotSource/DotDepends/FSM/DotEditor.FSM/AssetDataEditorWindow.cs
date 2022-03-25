using DotEngine.Core;
using DotEngine.FSM;
using DotEngine.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DotEditor.FSM
{
    public class AssetDataEditorWindow : EditorWindow
    {
        [MenuItem("Game/FSM/Asset Window &G")]
        public static void Open()
        {
            var win = EditorWindow.GetWindow<AssetDataEditorWindow>();
            win.titleContent = new GUIContent("Asset Window");
            win.Show();
        }

        void OnEnable()
        {
            var root = rootVisualElement;

            //BlackboardData data = new BlackboardData();

            //BlackboardView bbView = new BlackboardView();
            //bbView.BindedData = data;
            //root.Add(bbView);

            DataContainerView dcView = new DataContainerView();
            root.Add(dcView);

            //List<BlackboardData> datas = new List<BlackboardData>();
            //datas.Add(new BlackboardData() { Key = "name" });
            //datas.Add(new BlackboardData() { Key = "time" });
            //datas.Add(new BlackboardData() { Key = "time" });
            //datas.Add(new BlackboardData() { Key = "time" });
            //datas.Add(new BlackboardData() { Key = "time" });
            //datas.Add(new BlackboardData() { Key = "time" }); 
            //datas.Add(new BlackboardData() { Key = "time" });
            //datas.Add(new BlackboardData() { Key = "time" });
            //datas.Add(new BlackboardData() { Key = "time" });
            //datas.Add(new BlackboardData() { Key = "time" });
            //Func<VisualElement> makeItem = () => new BlackboardView();
            //Action<VisualElement, int> bindItem = (e, i) => (e as BlackboardView).BindedData= datas[i];

            //var m_DataListView = new ListView();
            //m_DataListView.SetBorderColor(new Color(1, 0, 0, 1));
            //m_DataListView.SetBorderWidth(2);
            //m_DataListView.SetMargin(4, LengthUnit.Pixel);
            //m_DataListView.SetPadding(2, LengthUnit.Pixel);
            //m_DataListView.SetHeight(100, LengthUnit.Percent);
            //m_DataListView.itemHeight = 40;
            //m_DataListView.makeItem = makeItem;
            //m_DataListView.bindItem = bindItem;
            //m_DataListView.itemsSource = datas;
            //m_DataListView.selectionType = SelectionType.Single;
            //m_DataListView.reorderable = true;
            //m_DataListView.RegisterCallback<GeometryChangedEvent>((e) =>
            //{
            //    m_DataListView.Refresh();
            //});

            //var sv = m_DataListView.Q<ScrollView>();
            //Debug.Log("FFFFFFFFFF," + sv.layout.height);
            //root.Add(m_DataListView);
        }

    }
}
