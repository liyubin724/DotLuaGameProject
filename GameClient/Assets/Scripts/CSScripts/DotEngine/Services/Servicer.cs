namespace DotEngine.Services
{
    public class Servicer : IServicer
    {
        public string Name { get; private set; }

        public Servicer(string name)
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
