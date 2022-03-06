using UnityEngine;
using UnityEngine.UIElements;
using LogLevel = DotEngine.Log.LogLevel;

namespace DotEditor.Log
{
    public class LogItemViewer : VisualElement
    {
        public static readonly string iconUssClassName = "unity-help-box__icon";
        public static readonly string iconInfoUssClassName = "unity-help-box__icon--info";
        public static readonly string iconwarningUssClassName = "unity-help-box__icon--warning";
        public static readonly string iconErrorUssClassName = "unity-help-box__icon--error";

        public static readonly Color textInfoColor = Color.white;
        public static readonly Color textWarningColor = Color.yellow;
        public static readonly Color textErrorColor = Color.red;

        public new class UxmlFactory : UxmlFactory<LogItemViewer, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
        }

        VisualElement m_Icon;
        string m_IconClass;

        Label m_TimeLabel;
        Label m_TagLabel;
        Label m_MessageLabel;
        public LogItemViewer()
        {
            style.flexDirection = FlexDirection.Row;
            style.alignItems = Align.Center;

            m_Icon = new VisualElement();
            m_Icon.AddToClassList(iconUssClassName);
            Add(m_Icon);

            m_TimeLabel = new Label();
            m_TimeLabel.text = string.Empty;
            Add(m_TimeLabel);

            m_TagLabel = new Label();
            m_TagLabel.text = string.Empty;
            Add(m_TagLabel);

            m_MessageLabel = new Label();
            m_MessageLabel.text = string.Empty;
            Add(m_MessageLabel);
        }

        public void SetItemData(LogData data)
        {
            UpdateIcon(data.Level);

            UpdateText(m_TimeLabel, data.Level, data.Time.ToString("yyyy-MM-dd HH:mm:ss FFF"));
            UpdateText(m_TagLabel, data.Level, data.Tag);
            UpdateText(m_MessageLabel, data.Level, data.Message);
        }

        private void UpdateIcon(LogLevel level)
        {
            if (!string.IsNullOrEmpty(m_IconClass))
            {
                m_Icon.RemoveFromClassList(m_IconClass);
            }
            m_IconClass = GetIconClass(level);
            m_Icon.AddToClassList(m_IconClass);
        }

        private string GetIconClass(LogLevel level)
        {
            if (level <= LogLevel.Info)
            {
                return iconInfoUssClassName;
            }
            else if (level == LogLevel.Warning)
            {
                return iconwarningUssClassName;
            }
            else
            {
                return iconErrorUssClassName;
            }
        }

        private void UpdateText(Label label, LogLevel level, string message)
        {
            Color textColor = GetTextColor(level);
            label.style.color = textColor;
            label.text = message;
        }

        private Color GetTextColor(LogLevel level)
        {
            if (level <= LogLevel.Info)
            {
                return textInfoColor;
            }
            else if (level == LogLevel.Warning)
            {
                return textWarningColor;
            }
            else
            {
                return textErrorColor;
            }
        }
    }
}
