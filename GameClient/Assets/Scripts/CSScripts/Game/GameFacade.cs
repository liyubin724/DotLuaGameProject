using DotEngine;
using DotEngine.Net.Services;
using IFacade = DotEngine.Framework.IFacade;
using Facade = DotEngine.Framework.Facade;

namespace Game
{
    public class GameFacade : Facade
    {
        public static new IFacade GetInstance()
        {
            if(facade == null)
            {
                facade = new GameFacade();
            }
            return facade;
        }


    }
}
