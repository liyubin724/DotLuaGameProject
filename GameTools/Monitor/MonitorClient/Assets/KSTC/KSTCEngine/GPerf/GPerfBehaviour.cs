using UnityEngine;

namespace KSTCEngine.GPerf
{
    public class GPerfBehaviour : MonoBehaviour
    {
        private void Update()
        {
            GPerfMonitor.GetInstance().DoUpdate(Time.deltaTime);
        }

        private void OnDestroy()
        {
            GPerfMonitor.GetInstance().DoDispose();
        }
    }
}
