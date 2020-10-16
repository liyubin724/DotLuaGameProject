using UnityEngine;

namespace KSTCEngine.GPerf
{
    public class GPerfBehaviour : MonoBehaviour
    {
        private void Update()
        {
            if(GPerfMonitor.Monitor!=null)
            {
                GPerfMonitor.Monitor.DoUpdate(Time.unscaledDeltaTime);
            }
        }

        private void OnDestroy()
        {
            if (GPerfMonitor.Monitor != null)
            {
                GPerfMonitor.ShuntDown();
            }
        }
    }
}
