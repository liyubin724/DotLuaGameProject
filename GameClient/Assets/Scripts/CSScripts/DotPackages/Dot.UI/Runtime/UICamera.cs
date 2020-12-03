using UnityEngine;

namespace DotEngine.UI
{
    [RequireComponent(typeof(Camera))]
    public class UICamera : MonoBehaviour
    {
        public Camera uiCamera;

        private void Awake()
        {
            if(uiCamera == null )
            {
                uiCamera = GetComponent<Camera>();
            }
        }

    }
}
