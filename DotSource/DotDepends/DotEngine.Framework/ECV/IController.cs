namespace DotEngine.Framework
{
    public interface IController
    {
        string Name { get; }
        bool Enable { get; set; }
        IEntity Entity { get; }

        void DoInitilize();
        void DoAttach(string name, object paramValue);
        void DoActive();
        void DoDeactive();
        void DoDettach();
        void DoDestroy();
    }
}
