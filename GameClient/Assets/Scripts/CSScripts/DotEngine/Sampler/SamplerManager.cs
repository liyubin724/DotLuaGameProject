using DotEngine.Utilities;
using UnityEngine;

namespace DotEngine.Sampler
{
    public class SamplerManager
    {
        private static SamplerManager mgr = null;

        public static SamplerManager GetInstance()
        {
            if(mgr == null)
            {
                mgr = new SamplerManager();
            }
            return mgr;
        }

        private SamplerBehaviour recorderBehaviour = null;

        private SamplerManager()
        { }

        public void Startup()
        {
            recorderBehaviour = DontDestroyHandler.CreateComponent<SamplerBehaviour>();
        }

        public void Shuntdown()
        {
            if(recorderBehaviour !=null)
            {
                GameObject.Destroy(recorderBehaviour.gameObject);
            }
            recorderBehaviour = null;
            mgr = null;
        }
    }
}
