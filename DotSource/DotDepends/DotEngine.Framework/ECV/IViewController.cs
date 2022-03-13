namespace DotEngine.Framework
{
    public interface IViewController : IController
    {
        IView View { get; set; }
    }
}
