using DotEngine.Log;
using DotEngine.Utilities;
using UnityEngine;

namespace Game
{
    public class StartupBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            DotEngine.Log.ILogger logger = new UnityLogger();
            LogUtil.SetLogger(logger);

            GameFacade.GetInstance();

            DontDestroyHandler.AddTransform(transform);
        }

        private void OnDestroy()
        {
            LogUtil.DisposeLogger();
        }
    }
}
