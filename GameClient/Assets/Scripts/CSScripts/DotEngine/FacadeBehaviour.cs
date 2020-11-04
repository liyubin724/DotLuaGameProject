using UnityEngine;

namespace DotEngine
{
    public class FacadeBehaviour : MonoBehaviour
    {
        private void Update()
        {
            Facade.GetInstance().DoUpdate(Time.deltaTime);
            Facade.GetInstance().DoUnscaleUpdate(Time.unscaledDeltaTime);
        }

        private void LateUpdate()
        {
            Facade.GetInstance().DoLateUpdate(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            Facade.GetInstance().DoFixedUpdate(Time.fixedDeltaTime);
        }
    }
}
