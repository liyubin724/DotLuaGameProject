namespace DotEngine.Framework.Services
{
    public interface IService
    {
        string Name { get; }
        bool Enable { get; set; }

        void DoRegistered();
        void DoUnregistered();
    }
}