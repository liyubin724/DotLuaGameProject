namespace DotEngine.Services
{
    public interface IServicer
    {
        string Name { get; }

        void DoRegister();
        void DoRemove();
    }
}
