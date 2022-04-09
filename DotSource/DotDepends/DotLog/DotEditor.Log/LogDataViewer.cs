using DotEngine.Log;
using DotEngine.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DotEditor.Log
{
    public class LogDataViewer : VisualElement
    {
        private const float VIEWER_HEIGHT = 40.0f;

        private LogFlagViewer m_FlagViewer;
        private Label m_DatetimeLabel;
        private Label m_TagLabel;
        private TextField m_MessageText;

        private LogData m_BindedData;
        public LogData BindedData
        {
            get
            {
                return m_BindedData;
            }
            set
            {
                m_BindedData = value;
                UpdateViewer();
            }
        }

        public LogDataViewer()
        {
            this.SetWidth(100, LengthUnit.Percent);
            this.SetHeight(VIEWER_HEIGHT);
            this.SetColumn();

            var container = new VisualElement();
            container.SetRow();
            container.ExpandWidthAndHeight();
            Add(container);

            m_FlagViewer = new LogFlagViewer();
            m_FlagViewer.SetWidth(VIEWER_HEIGHT);
            m_FlagViewer.SetHeight(VIEWER_HEIGHT);
            container.Add(m_FlagViewer);

            m_DatetimeLabel = new Label();
            m_DatetimeLabel.SetWidth(220);
            m_DatetimeLabel.SetTextAlign(TextAnchor.MiddleCenter);
            m_DatetimeLabel.SetFontSize(18,LengthUnit.Pixel);
            container.Add(m_DatetimeLabel);

            m_TagLabel = new Label();
            m_TagLabel.SetWidth(80);
            m_TagLabel.SetTextAlign(TextAnchor.MiddleCenter);
            m_TagLabel.SetFontSize(18);
            m_TagLabel.SetFontStyle(FontStyle.Bold);
            container.Add(m_TagLabel);

            m_MessageText = new TextField();
            m_MessageText.label = null;
            m_MessageText.isReadOnly = true;
            m_MessageText.SetWidth(100, LengthUnit.Percent);
            container.Add(m_MessageText);

            VisualElement hLine = new VisualElement();
            hLine.SetMargin(2, LengthUnit.Pixel);
            hLine.SetHeight(2);
            hLine.SetWidth(100, LengthUnit.Percent);
            hLine.SetBackgroundColor(Color.grey);
            Add(hLine);
        }

        public void UpdateViewer()
        {
            m_FlagViewer.Level = m_BindedData == null ? LogLevel.Off : m_BindedData.Level;
            m_DatetimeLabel.text = m_BindedData == null ? string.Empty : m_BindedData.Time.ToString("HH:mm:ss fff");
            m_TagLabel.text = m_BindedData == null ? string.Empty : m_BindedData.Tag;
            m_MessageText.value = m_BindedData == null ? string.Empty : m_BindedData.Message;
        }
    }
}
