using DotEngine.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DotEditor.Log
{
    public class LogDataListViewer : VisualElement
    {
        private static readonly string ussClassName = "unity-toolbar-button";

        private List<LogData> m_BindedDatas;
        public List<LogData> BindedData
        {
            get
            {
                return m_BindedDatas;
            }
            set
            {
                m_BindedDatas = value;
                UpdateViewer();
            }
        }

        public event Action<LogData> OnDataSelected;

        private ListView m_ListView;
        public LogDataListViewer()
        {
            this.SetBorderColor(Color.grey);
            this.SetBorderWidth(2);
            
            DrawListHeader();
            DrawListView();
        }

        public void UpdateViewer()
        {
            m_ListView.itemsSource = m_BindedDatas;
            m_ListView.Refresh();
        }

        private void DrawListView()
        {
            m_ListView = new ListView();
            m_ListView.ExpandWidthAndHeight();
            Add(m_ListView);

            Func<VisualElement> makeItemFunc = () =>
            {
                return new LogDataViewer();
            };
            Action<VisualElement, int> bindItemAction = (ve, index) =>
             {
                 var dataViewer = ve as LogDataViewer;
                 dataViewer.BindedData = m_BindedDatas[index];
             };
            Action<VisualElement, int> unbindItemAction = (ve, index) =>
             {
                 var dataViewer = ve as LogDataViewer;
                 dataViewer.BindedData = null;
             };
            m_ListView.makeItem = makeItemFunc;
            m_ListView.bindItem = bindItemAction;
            m_ListView.unbindItem = unbindItemAction;
            m_ListView.selectionType = SelectionType.Single;
            m_ListView.onSelectionChange += (items) =>
            {
                LogData logData = (items.ToArray()[0]) as LogData;
                OnDataSelected?.Invoke(logData);
            };
        }

        private void DrawListHeader()
        {
            Toolbar headerTitle = new Toolbar();
            Add(headerTitle);

            Label flagHeader = new Label();
            flagHeader.AddToClassList(ussClassName);
            flagHeader.SetWidth(40);
            headerTitle.Add(flagHeader);

            Label datetimeHeader = new Label();
            datetimeHeader.text = "Datetime";
            datetimeHeader.AddToClassList(ussClassName);
            datetimeHeader.SetWidth(220);
            headerTitle.Add(datetimeHeader);

            Label tagHeader = new Label();
            tagHeader.text = "Tag";
            tagHeader.AddToClassList(ussClassName);
            tagHeader.SetWidth(80);
            headerTitle.Add(tagHeader);

            Label messageHeader = new Label();
            messageHeader.text = "Message";
            messageHeader.AddToClassList(ussClassName);
            messageHeader.ExpandWidth();
            headerTitle.Add(messageHeader);

            Add(headerTitle);
        }
    }
}
