using UnityEngine;

namespace DotEngine.Core.Update
{
    public class UpdateBehaviour : MonoBehaviour
    {
        void Update()
        {
            UpdateManager.GetInstance().DoUpdate(Time.deltaTime, Time.unscaledDeltaTime);
        }

        void LateUpdate()
        {
            UpdateManager.GetInstance().DoLateUpdate(Time.deltaTime, Time.unscaledDeltaTime);
        }

        void FixedUpdate()
        {
            UpdateManager.GetInstance().DoFixedUpdate(Time.fixedDeltaTime, Time.fixedUnscaledDeltaTime);
        }
    }
}
