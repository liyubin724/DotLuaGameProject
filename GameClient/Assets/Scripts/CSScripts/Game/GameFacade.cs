using DotEngine;
using DotEngine.Lua;

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

        protected override void InitializeService()
        {
            base.InitializeService();

        }
    }
}
