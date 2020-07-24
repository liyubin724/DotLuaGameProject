namespace DotEngine.Services
{
    public class Service : IService
    {
        public string Name { get; private set; }

        public Service(string name)
        {
            Name = name;
        }

        public virtual void DoRegister()
        {
        }

        public virtual void DoRemove()
        {
        }
    }
}
