namespace DotEngine.Services
{
    public interface IService
    {
        string Name { get; }

        void DoRegister();
        void DoRemove();
    }
}
