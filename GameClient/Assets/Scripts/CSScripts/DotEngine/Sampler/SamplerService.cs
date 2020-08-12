using Boo.Lang;
using DotEngine.Services;

namespace DotEngine.Sampler
{
    public class SamplerService : Service , IUpdate
    {
        public static string NAME = "SamplerService";

        private List<ASampler> samplers = new List<ASampler>();

        private SamplerService():base(NAME)
        {

        }

        public void DoUpdate(float deltaTime)
        {
            
        }
    }
}
