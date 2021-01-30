using UnityEngine;

namespace DotEngine
{
    public class FacadeBehaviour : MonoBehaviour
    {
        private void Update()
        {
            Facade.GetInstance().DoUpdate(Time.deltaTime, Time.unscaledDeltaTime);
        }

        private void LateUpdate()
        {
            Facade.GetInstance().DoLateUpdate(Time.deltaTime, Time.unscaledDeltaTime);
        }

        private void FixedUpdate()
        {
            Facade.GetInstance().DoFixedUpdate(Time.fixedDeltaTime, Time.fixedUnscaledDeltaTime);
        }
    }
}
