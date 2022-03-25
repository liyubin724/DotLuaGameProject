using DotEngine.FSM;
using DotEngine.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace DotEditor.FSM
{
    public class DataContainerView : VisualElement
    {
        public class Factory : UxmlFactory<MachineView, Traits> { }
        public class Traits : UxmlTraits { }

        [Q("data-header")]
        private Label m_HeaderLabel;
        [Q("data-list")]
        private ListView m_DataListView;

        private List<BlackboardData> datas = new List<BlackboardData>();

        public DataContainerView()
        {
            this.SetHeight(100, LengthUnit.Percent);

            var visualTreeAsset = Resources.Load<VisualTreeAsset>("dot_fsm_data_list_uxml");
            var visualTree = visualTreeAsset.CloneTree();
            Add(visualTree);

            visualTree.AssignQueryResults(this);
            visualTree.SetHeight(100, LengthUnit.Percent);
            m_HeaderLabel.text = "Blackboard Data List";

            datas.Add(new BlackboardData() { Key = "name" });
            datas.Add(new BlackboardData() { Key = "time" });
            Func<VisualElement> makeItem = () => new BlackboardView();
            Action<VisualElement, int> bindItem = (e, i) => (e as BlackboardView).BindedData = datas[i];

            m_DataListView.itemHeight = 40;
            m_DataListView.makeItem = makeItem;
            m_DataListView.bindItem = bindItem;
            m_DataListView.itemsSource = datas;
            m_DataListView.onItemsChosen += Debug.Log;
            m_DataListView.onSelectionChange += Debug.Log;
            m_DataListView.RegisterCallback<GeometryChangedEvent>((e) =>
            {
                m_DataListView.Refresh();
            });

            var sv = m_DataListView.Q<ScrollView>();
            Debug.Log("FFFFFFFFFF," + sv.layout.height);
        }
    }
}
