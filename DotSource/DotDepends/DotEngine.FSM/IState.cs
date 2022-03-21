namespace DotEngine.FSM
{
    public interface IState
    {
        string Name { get; set; }

        void DoInitilize(Machine machine);
        void DoEnter(string from);
        void DoUpdate(float deltaTime);
        void DoLeave(string to);
        void DoDestroy();
    }
}
