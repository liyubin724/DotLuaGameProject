namespace DotEngine.Framework.Services
{
    public abstract class Service : IService
    {
        public string Name { get; private set; }

        public bool Enable { get; set; } = true;

        public Service(string name)
        {
            Name = name;
        }

        public abstract void DoRegistered();
        public abstract void DoUnregistered();
    }
}
