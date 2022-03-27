using DotEngine.Config.Ini;
using DotEngine.UIElements;
using System;
using UnityEngine.UIElements;

namespace DotEditor.Config.Ini
{
    public class IniPropertyView : VisualElement
    {
        private HelpBox m_HelpBox;

        private TextField m_CommentField;
        private TextField m_OptionalValueField;
        private TextField m_KeyField;
        private TextField m_ValueField;

        private IniProperty m_PropertyData;
        public IniProperty BindData
        {
            get
            {
                return m_PropertyData;
            }
            set
            {
                if(m_PropertyData!=value)
                {
                    m_PropertyData = value;
                    Refresh();
                }
            }
        }

        public IniPropertyView()
        {
            //m_HelpBox = new HelpBox();
            //m_HelpBox.SetVisibility(Visibility.Hidden);
            //Add(m_HelpBox);

            m_CommentField = new TextField();
            m_CommentField.label = "Comments:";
            m_CommentField.multiline = true;
            m_CommentField.SetHeight(80);
            m_CommentField.RegisterValueChangedCallback((e) =>
            {
                if(m_PropertyData == null)
                {
                    return;
                }
                m_PropertyData.Comments.Clear();
                string value = e.newValue;
                if (!string.IsNullOrEmpty(value))
                {
                    string[] values = value.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);    
                    if(values!=null && values.Length>0)
                    {
                        m_PropertyData.Comments.AddRange(values);
                    }
                }
            });
            Add(m_CommentField);

            m_OptionalValueField = new TextField();
            m_OptionalValueField.label = "OptionalValues:";
            m_OptionalValueField.RegisterValueChangedCallback((e) =>
            {
                if (m_PropertyData == null)
                {
                    return;
                }
                m_PropertyData.OptionalValues.Clear();
                string value = e.newValue;
                if(!string.IsNullOrEmpty(value))
                {
                    string[] values = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                    if (values != null && values.Length > 0)
                    {
                        m_PropertyData.OptionalValues.AddRange(values);
                    }
                }
            });
            Add(m_OptionalValueField);

            m_KeyField = new TextField();
            m_KeyField.label = "Key:";
            m_KeyField.RegisterValueChangedCallback((e) =>
            {
                if (m_PropertyData == null)
                {
                    return;
                }
                m_PropertyData.Key = e.newValue;
            });
            Add(m_KeyField);

            m_ValueField = new TextField();
            m_ValueField.label = "Value:";
            m_ValueField.RegisterValueChangedCallback((e) =>
            {
                if (m_PropertyData == null)
                {
                    return;
                }
                m_PropertyData.Value = e.newValue;
            });
            Add(m_ValueField);
        }

        private void ShowHelpBox(HelpBoxMessageType messageType,string message)
        {
            m_HelpBox.SetVisibility(Visibility.Visible);
            MarkDirtyRepaint();
        }

        private void Refresh()
        {
            if (m_PropertyData != null)
            {
                m_CommentField.value = string.Join("\r\n", m_PropertyData.Comments.ToArray());
                m_OptionalValueField.value = string.Join(";", m_PropertyData.OptionalValues.ToArray());
                m_KeyField.value = m_PropertyData.Key;
                m_ValueField.value = m_PropertyData.Value;
            }
            else
            {
                m_CommentField.value = string.Empty;
                m_OptionalValueField.value = string.Empty;
                m_KeyField.value = string.Empty;
                m_ValueField.value = string.Empty;
            }
        }
    }
}
