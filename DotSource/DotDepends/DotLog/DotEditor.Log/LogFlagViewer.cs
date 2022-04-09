using DotEngine.Log;
using UnityEngine.UIElements;

namespace DotEditor.Log
{
    public class LogFlagViewer : VisualElement
    {
        private static readonly string iconUssClassName = "unity-help-box__icon";
        private static readonly string iconInfoUssClassName = "unity-help-box__icon--info";
        private static readonly string iconWarningUssClassName = "unity-help-box__icon--warning";
        private static readonly string iconErrorUssClassName = "unity-help-box__icon--error";

        public class Factory : UxmlFactory<LogFlagViewer,Traits>
        {
        }

        public class Traits : UxmlTraits
        {
            UxmlEnumAttributeDescription<LogLevel> m_Level = new UxmlEnumAttributeDescription<LogLevel>() { name = "level", defaultValue = LogLevel.Off };

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                var viewer = ve as LogFlagViewer;
                viewer.Level = m_Level.GetValueFromBag(bag, cc);
            }
        }

        VisualElement m_Icon;
        string m_IconClass;

        private LogLevel m_Level = LogLevel.Off;
        public LogLevel Level
        {
            get
            {
                return m_Level;
            }
            set
            {
                m_Level = value;
                UpdateIcon();
            }
        }

        public LogFlagViewer()
        {
            m_Icon = new VisualElement();
            m_Icon.AddToClassList(iconUssClassName);
            Add(m_Icon);
        }

        private void UpdateIcon()
        {
            if (!string.IsNullOrEmpty(m_IconClass))
            {
                m_Icon.RemoveFromClassList(m_IconClass);
                m_IconClass = null;
            }

            if(m_Level>LogLevel.Off && m_Level<=LogLevel.Info)
            {
                m_IconClass = iconInfoUssClassName;
            }else if(m_Level == LogLevel.Warning)
            {
                m_IconClass = iconWarningUssClassName;
            }
            else
            {
                m_IconClass = iconErrorUssClassName;
            }
            if(!string.IsNullOrEmpty(m_IconClass))
            {
                m_Icon.AddToClassList(m_IconClass);
            }
        }
    }
}
