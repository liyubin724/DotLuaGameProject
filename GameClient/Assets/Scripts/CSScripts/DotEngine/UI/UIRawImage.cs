using DotEngine.Asset;
using DotEngine.Log;
using UnityEngine;
using UnityEngine.UI;

namespace DotEngine.UI
{
    public class UIRawImage : RawImage
    {
        [SerializeField]
        private string m_ImageAddress;
        public string ImageAddress
        {
            get
            {
                return m_ImageAddress;
            }
            set
            {
                if(m_ImageAddress!=value)
                {
                    m_ImageAddress = value;
                    LoadImage();
                }
            }
        }

        [SerializeField]
        private bool m_IsSetNativeSize = true;
        public bool IsSetNativeSize
        {
            get
            {
                return m_IsSetNativeSize;
            }
            set
            {
                if(m_IsSetNativeSize!=value)
                {
                    m_IsSetNativeSize = value;
                    if(texture!=null)
                    {
                        SetNativeSize();
                    }
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            
            LoadImage();
        }

        private AssetHandler assetHandler = null;
        private void LoadImage()
        {
            if(Application.isPlaying)
            {
                AssetService assetService = Facade.GetInstance().GetService<AssetService>(AssetService.NAME);
                if(assetHandler!=null)
                {
                    assetService.UnloadAssetAsync(assetHandler);
                    assetHandler = null;
                }
                if(texture !=null && string.IsNullOrEmpty(m_ImageAddress))
                {
                    texture = null;
                }else if(!string.IsNullOrEmpty(m_ImageAddress))
                {
                    assetHandler = assetService.LoadAssetAsync(m_ImageAddress, OnLoadComplete);
                }
            }
        }

        private void OnLoadComplete(string address, Object uObj, object userData)
        {
            assetHandler = null;

            Texture2D tex = (Texture2D)uObj;
            if(tex != null)
            {
                texture = tex;
                if(m_IsSetNativeSize)
                {
                    SetNativeSize();
                }
            }else
            {
                LogUtil.LogError("UIRawImage", "UIRawImage::OnLoadComplete->uObject is null.address = " + address);
            }
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            if(texture == null)
            {
                vh.Clear();
            }
            else
            {
                base.OnPopulateMesh(vh);
            }
        }

        protected override void OnDestroy()
        {
            if(assetHandler!=null)
            {
                AssetService assetService = Facade.GetInstance().GetService<AssetService>(AssetService.NAME);
                assetService.UnloadAssetAsync(assetHandler);
                assetHandler = null;
            }

            base.OnDestroy();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            LoadImage();
        }
#endif
    }
}