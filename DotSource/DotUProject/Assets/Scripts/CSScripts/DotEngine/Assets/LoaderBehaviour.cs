using UnityEngine;

namespace DotEngine.Assets
{
    public class LoaderBehaviour : MonoBehaviour
    {
        private ILoader loader = null;

        internal void SetLoader(ILoader l)
        {
            loader = l;
        }

        private void Update()
        {
            if(loader!=null)
            {
                loader.DoUdpate(Time.deltaTime, Time.unscaledDeltaTime);
            }
        }
    }
}
