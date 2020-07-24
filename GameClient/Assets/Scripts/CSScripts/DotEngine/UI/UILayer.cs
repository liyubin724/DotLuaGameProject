using UnityEngine;

namespace DotEngine.UI
{
    public enum UILayerLevel
    {
        BottomlowestLayer = 0,
        BottomLayer,
        DefaultLayer,
        TopLayer,
        TopmostLayer,
    }

    public class UILayer : MonoBehaviour
    {
        public UILayerLevel layerLevel = UILayerLevel.DefaultLayer;

        public RectTransform LayerTransform { get; private set; }
        public GameObject LayerGameObject { get; private set; }

        private bool m_Visible = true;
        public bool Visible
        {
            get
            {
                return m_Visible;
            }
            set
            {
                if(m_Visible!=value)
                {
                    m_Visible = value;
                    LayerGameObject.SetActive(m_Visible);
                }
            }
        }

        private void Awake()
        {
            LayerGameObject = gameObject;
            LayerTransform = (RectTransform)transform;
        }
    }
}
