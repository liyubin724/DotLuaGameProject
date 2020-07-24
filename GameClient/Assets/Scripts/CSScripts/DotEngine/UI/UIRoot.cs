using DotEngine.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace DotEngine.UI
{
    public class UIRoot : MonoBehaviour
    {
        public static UIRoot Root = null;

        public UICamera uiCamera;
        public UILayer[] layers = new UILayer[0];

        private Dictionary<UILayerLevel, UILayer> layerDic = new Dictionary<UILayerLevel, UILayer>();
        private void Awake()
        {
            Root = this;
            DontDestroyHandler.AddTransform(transform);

            foreach(var layer in layers)
            {
                layerDic.Add(layer.layerLevel, layer);
            }
        }

        public UILayer GetLayer(UILayerLevel layerLevel)
        {
            if(layerDic.TryGetValue(layerLevel,out UILayer layer))
            {
                return layer;
            }
            return null;
        }
    }
}
