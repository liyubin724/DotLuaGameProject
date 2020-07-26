using DotEngine;
using DotEngine.Asset;

namespace Game
{
    public class GameFacade : Facade
    {
        public new static Facade GetInstance()
        {
            if(instance == null)
            {
                instance = new GameFacade();
            }
            return instance;
        }

        protected override void InitializeFacade()
        {
            base.InitializeFacade();
        }

        protected override void InitializeService()
        {
            base.InitializeService();
        }
    }
}
